using GymBudgetApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GymBudgetApp.Services
{
    public class PaymentReminderService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<PaymentReminderService> _logger;
        private readonly IConfiguration _configuration;

        public PaymentReminderService(
            IServiceScopeFactory scopeFactory,
            ILogger<PaymentReminderService> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wait a bit on startup so the app can finish initializing
            await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await SendReminders(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Payment reminder check failed");
                }

                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }

        private async Task SendReminders(CancellationToken ct)
        {
            var enabled = _configuration.GetValue<bool>("PaymentReminders:Enabled");
            if (!enabled)
            {
                _logger.LogInformation("Payment reminders are disabled");
                return;
            }

            var daysBeforeDue = _configuration.GetValue("PaymentReminders:DaysBeforeDue", 3);
            var sendPastDue = _configuration.GetValue("PaymentReminders:SendPastDueReminders", true);
            var appUrl = _configuration["AppUrl"] ?? "https://gymbudgetapp-production.up.railway.app";

            using var scope = _scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            var publishedSeasons = await db.Seasons.AsNoTracking()
                .Where(s => s.IsPublished && s.IsActive)
                .ToListAsync(ct);

            if (!publishedSeasons.Any()) return;

            var parentLinks = await db.ParentLinks.AsNoTracking()
                .Where(pl => pl.IsClaimed && !pl.UseExternalBilling)
                .ToListAsync(ct);

            var athleteIds = parentLinks.Select(pl => pl.AthleteId).Distinct().ToList();
            var gymnasts = await db.Gymnasts.AsNoTracking()
                .Where(g => athleteIds.Contains(g.Id))
                .ToListAsync(ct);

            var payments = await db.Payments.AsNoTracking()
                .Where(p => p.Status == PaymentStatus.Paid && athleteIds.Contains(p.AthleteId))
                .ToListAsync(ct);

            var existingReminders = await db.PaymentReminderLogs.AsNoTracking()
                .Where(r => r.SentAt > DateTime.UtcNow.AddDays(-30))
                .ToListAsync(ct);

            // Resolve parent emails via Identity
            var parentUserIds = parentLinks.Select(pl => pl.ParentUserId).Where(id => id != null).Distinct().ToList();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var users = new List<IdentityUser>();
            foreach (var uid in parentUserIds)
            {
                var user = await userManager.FindByIdAsync(uid!);
                if (user != null) users.Add(user);
            }
            var parentEmails = users.Where(u => u.Email != null).ToDictionary(u => u.Id, u => u.Email!);

            // Get budget totals per season
            var budgetTotals = new Dictionary<int, decimal>();
            var athleteCounts = new Dictionary<int, int>();
            foreach (var season in publishedSeasons)
            {
                var total = await db.BudgetCalculatorEntries.AsNoTracking()
                    .Where(e => e.SeasonId == season.Id)
                    .SumAsync(e => (decimal?)e.CalculatedTotal, ct) ?? 0;
                budgetTotals[season.Id] = total;
                var count = season.AthleteCount > 0 ? season.AthleteCount
                    : await db.Gymnasts.AsNoTracking().CountAsync(ct);
                athleteCounts[season.Id] = count;
            }

            var emailSender = scope.ServiceProvider.GetRequiredService<IEmailSender>();

                foreach (var season in publishedSeasons)
                {
                    var costPerGymnast = athleteCounts[season.Id] > 0
                        ? Math.Round(budgetTotals[season.Id] / athleteCounts[season.Id], 2)
                        : 0;

                    foreach (var link in parentLinks)
                    {
                        if (ct.IsCancellationRequested) return;
                        if (link.ParentUserId == null || !parentEmails.ContainsKey(link.ParentUserId)) continue;

                        var gymnast = gymnasts.FirstOrDefault(g => g.Id == link.AthleteId);
                        if (gymnast == null) continue;

                        var athletePayments = payments
                            .Where(p => p.AthleteId == link.AthleteId && p.SeasonId == season.Id)
                            .ToList();
                        var totalPaid = athletePayments.Where(p => p.Type == PaymentType.Payment).Sum(p => p.Amount);
                        var totalCredits = athletePayments.Where(p => p.Type == PaymentType.Credit).Sum(p => p.Amount);
                        var totalCost = costPerGymnast; // Simplified — doesn't include apparel/comp for now

                        var dueInfo = ComputeNextDue(season, gymnast, totalCost, totalPaid, totalCredits);
                        if (dueInfo == null || !dueInfo.DueDate.HasValue) continue;

                        var daysUntilDue = (dueInfo.DueDate.Value - DateTime.Today).Days;
                        string? reminderType = null;

                        if (daysUntilDue == daysBeforeDue)
                            reminderType = "Upcoming";
                        else if (daysUntilDue == 0)
                            reminderType = "DueToday";
                        else if (daysUntilDue < 0 && sendPastDue && daysUntilDue >= -7)
                            reminderType = "PastDue";

                        if (reminderType == null) continue;

                        // Check if already sent
                        var alreadySent = existingReminders.Any(r =>
                            r.AthleteId == link.AthleteId
                            && r.ParentUserId == link.ParentUserId
                            && r.SeasonId == season.Id
                            && r.InstallmentNumber == dueInfo.InstallmentNumber
                            && r.ReminderType == reminderType);
                        if (alreadySent) continue;

                        var email = parentEmails[link.ParentUserId];
                        var subject = reminderType switch
                        {
                            "Upcoming" => $"Payment reminder: ${dueInfo.Amount:N2} due {dueInfo.DueDate.Value:MMM dd} for {gymnast.Name}",
                            "DueToday" => $"Payment due today: ${dueInfo.Amount:N2} for {gymnast.Name}",
                            "PastDue" => $"Past due: ${dueInfo.Amount:N2} was due {dueInfo.DueDate.Value:MMM dd} for {gymnast.Name}",
                            _ => "Payment reminder"
                        };

                        var html = BuildEmailHtml(gymnast.Name, dueInfo.Amount, dueInfo.DueDate.Value,
                            reminderType, dueInfo.RemainingInstallments, season.Name, appUrl);

                        try
                        {
                            await emailSender.SendEmailAsync(email, subject, html);
                            _logger.LogInformation("Sent {Type} reminder to {Email} for athlete {Athlete} season {Season}",
                                reminderType, email, gymnast.Name, season.Name);

                            db.PaymentReminderLogs.Add(new PaymentReminderLog
                            {
                                AthleteId = link.AthleteId,
                                ParentUserId = link.ParentUserId,
                                SeasonId = season.Id,
                                InstallmentNumber = dueInfo.InstallmentNumber,
                                ReminderType = reminderType
                            });
                            await db.SaveChangesAsync(ct);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send reminder to {Email}", email);
                        }
                    }
                }
        }

        private record DueInfo(DateTime? DueDate, decimal Amount, int InstallmentNumber, int RemainingInstallments);

        private static DueInfo? ComputeNextDue(Season season, Gymnast gymnast, decimal totalCost, decimal totalPaid, decimal totalCredits)
        {
            var balance = totalCost - totalPaid - totalCredits;
            if (balance <= 0) return null;

            var planMonths = gymnast.PaymentPlanMonths ?? season.PaymentMonths;
            if (planMonths <= 0) planMonths = 8;

            var startMonth = gymnast.PaymentStartMonth ?? (season.PaymentStartMonth > 0 ? season.PaymentStartMonth : 8);
            var scheduledTotal = Math.Max(totalCost - totalCredits, 0);
            var monthlyAmount = planMonths > 0 ? Math.Round(scheduledTotal / planMonths, 2) : balance;

            var scheduleYear = GetScheduleYear(season, startMonth);
            var scheduleStart = new DateTime(scheduleYear, startMonth, 15);

            for (int i = 0; i < planMonths; i++)
            {
                var dueDate = scheduleStart.AddMonths(i);
                var cumulativeDue = monthlyAmount * (i + 1);
                if (totalPaid >= cumulativeDue) continue;

                return new DueInfo(dueDate, monthlyAmount, i + 1, planMonths - i);
            }

            return null;
        }

        private static int GetScheduleYear(Season season, int startMonthNum)
        {
            var match = Regex.Match(season.Name ?? "", @"\b(20\d{2})\b");
            if (match.Success && int.TryParse(match.Groups[1].Value, out var seasonYear))
                return seasonYear;
            return startMonthNum <= DateTime.Today.Month ? DateTime.Today.Year : DateTime.Today.Year - 1;
        }

        private static string BuildEmailHtml(string athleteName, decimal amount, DateTime dueDate,
            string reminderType, int remaining, string seasonName, string appUrl)
        {
            var statusColor = reminderType switch
            {
                "PastDue" => "#dc3545",
                "DueToday" => "#ffc107",
                _ => "#0d6efd"
            };
            var statusText = reminderType switch
            {
                "PastDue" => "PAST DUE",
                "DueToday" => "DUE TODAY",
                _ => "UPCOMING"
            };

            return $@"
<div style=""max-width:600px;margin:0 auto;font-family:Arial,sans-serif;"">
    <div style=""background:#1a6b3c;padding:20px;text-align:center;"">
        <h1 style=""color:white;margin:0;font-size:24px;"">Top Notch Training</h1>
    </div>
    <div style=""padding:20px;background:#f8f9fa;"">
        <div style=""background:white;border-radius:8px;padding:20px;margin-bottom:15px;"">
            <div style=""display:inline-block;background:{statusColor};color:white;padding:4px 12px;border-radius:4px;font-size:12px;font-weight:bold;margin-bottom:10px;"">{statusText}</div>
            <h2 style=""margin:10px 0 5px;color:#333;"">Payment Reminder for {athleteName}</h2>
            <p style=""color:#666;margin:0 0 15px;"">{seasonName}</p>
            <div style=""background:#f8f9fa;border-radius:8px;padding:15px;text-align:center;"">
                <div style=""font-size:32px;font-weight:bold;color:#333;"">${amount:N2}</div>
                <div style=""color:#666;margin-top:5px;"">Due {dueDate:MMMM d, yyyy}</div>
                <div style=""color:#999;font-size:13px;margin-top:3px;"">{remaining} payment(s) remaining</div>
            </div>
        </div>
        <div style=""text-align:center;margin-top:20px;"">
            <a href=""{appUrl}/parent"" style=""display:inline-block;background:#1a6b3c;color:white;padding:12px 30px;border-radius:6px;text-decoration:none;font-weight:bold;"">View Dashboard & Pay</a>
        </div>
        <p style=""color:#999;font-size:12px;text-align:center;margin-top:20px;"">
            This is an automated reminder from Top Notch Training's GymHub portal.
        </p>
    </div>
</div>";
        }
    }
}
