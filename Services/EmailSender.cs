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
                ?? _configuration["Smtp:Email"]
                ?? throw new InvalidOperationException("SMTP email is not configured. Set SMTP_EMAIL env var or Smtp:Email in configuration.");

            var smtpPassword = Environment.GetEnvironmentVariable("SMTP_PASSWORD")
                ?? _configuration["Smtp:Password"]
                ?? throw new InvalidOperationException("SMTP password is not configured. Set SMTP_PASSWORD env var or Smtp:Password in configuration.");

            using var client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(smtpEmail, smtpPassword),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };
            mailMessage.To.Add(email);

            try
            {
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Email sent to {Email} with subject '{Subject}'", email, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                throw;
            }
        }
    }
}
