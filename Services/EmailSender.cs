using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace GymBudgetApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpEmail = Environment.GetEnvironmentVariable("SMTP_EMAIL")
                ?? _configuration["Smtp:Email"] ?? "";
            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                ?? _configuration["Smtp:Password"] ?? "";

            if (string.IsNullOrEmpty(smtpEmail) || string.IsNullOrEmpty(smtpPassword))
            {
                _logger.LogWarning("SMTP not configured. Email to {Email} with subject '{Subject}' was not sent.", email, subject);
                return;
            }

            var smtpHost = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "smtp.gmail.com";
            var smtpPort = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 587;

            _logger.LogInformation("Attempting to send email to {Email} via {Host}:{Port} from {From}", email, smtpHost, smtpPort, smtpEmail);

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpEmail, "Top Notch Training"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent successfully to {Email}", email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email} via {Host}:{Port}", email, smtpHost, smtpPort);
                throw;
            }
        }
    }
}
