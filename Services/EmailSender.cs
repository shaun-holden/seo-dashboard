using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            var apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                ?? _configuration["SendGrid:ApiKey"] ?? "";
            var fromEmail = Environment.GetEnvironmentVariable("SENDGRID_FROM_EMAIL")
                ?? _configuration["SendGrid:FromEmail"] ?? "deshaun@tntgym.org";

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("SendGrid not configured. Email to {Email} with subject '{Subject}' was not sent.", email, subject);
                return;
            }

            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Top Notch Training");
            var to = new EmailAddress(email);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlMessage);

            try
            {
                var response = await client.SendEmailAsync(msg);
                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent to {Email} with subject '{Subject}'", email, subject);
                }
                else
                {
                    var body = await response.Body.ReadAsStringAsync();
                    _logger.LogError("SendGrid failed ({StatusCode}): {Body}", response.StatusCode, body);
                    throw new Exception($"SendGrid error: {response.StatusCode} - {body}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}", email);
                throw;
            }
        }
    }
}
