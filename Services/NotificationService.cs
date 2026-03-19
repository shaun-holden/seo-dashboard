using Microsoft.AspNetCore.Identity.UI.Services;

namespace GymBudgetApp.Services
{
    public class NotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(IEmailSender emailSender, IConfiguration configuration, ILogger<NotificationService> logger)
        {
            _emailSender = emailSender;
            _configuration = configuration;
            _logger = logger;
        }

        private List<string> GetNotificationEmails()
        {
            var emails = _configuration.GetSection("NotificationEmails").Get<List<string>>();
            return emails ?? new List<string>();
        }

        public async Task NotifyItemChange(string athleteName, string itemName, decimal itemCost, bool selected, decimal newBalance)
        {
            var action = selected ? "selected" : "deselected";
            var subject = $"{athleteName} {action} {itemName}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h3>Optional Item {(selected ? "Selected" : "Deselected")}</h3>
                    <p><strong>{athleteName}</strong> has {action} <strong>{itemName}</strong> (${itemCost:N2}).</p>
                    <p>New balance: <strong>${newBalance:N2}</strong></p>
                    <hr style='border: none; border-top: 1px solid #dee2e6;' />
                    <p style='color: #6c757d; font-size: 0.9em;'>Top Notch Training</p>
                </div>";

            await SendToAll(subject, body);
        }

        public async Task NotifyPayment(string athleteName, decimal amount, decimal remainingBalance)
        {
            var subject = $"Payment received — {athleteName}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h3>Payment Received</h3>
                    <p>A payment of <strong>${amount:N2}</strong> has been received for <strong>{athleteName}</strong>.</p>
                    <p>Remaining balance: <strong>${remainingBalance:N2}</strong></p>
                    <hr style='border: none; border-top: 1px solid #dee2e6;' />
                    <p style='color: #6c757d; font-size: 0.9em;'>Top Notch Training</p>
                </div>";

            await SendToAll(subject, body);
        }

        public async Task NotifyAutoPayEnabled(string athleteName, string parentEmail, decimal monthlyAmount)
        {
            var subject = $"Auto-Pay enabled — {athleteName}";
            var body = $@"
                <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;'>
                    <h3>Auto-Pay Enabled</h3>
                    <p><strong>{parentEmail}</strong> has enabled auto-pay for <strong>{athleteName}</strong>.</p>
                    <p>Monthly amount: <strong>${monthlyAmount:N2}</strong> on the 15th of each month.</p>
                    <hr style='border: none; border-top: 1px solid #dee2e6;' />
                    <p style='color: #6c757d; font-size: 0.9em;'>Top Notch Training</p>
                </div>";

            await SendToAll(subject, body);
        }

        private async Task SendToAll(string subject, string body)
        {
            foreach (var email in GetNotificationEmails())
            {
                try
                {
                    await _emailSender.SendEmailAsync(email, subject, body);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send notification to {Email}", email);
                }
            }
        }
    }
}
