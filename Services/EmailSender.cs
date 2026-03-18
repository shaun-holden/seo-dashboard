using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
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
            var apiKey = Environment.GetEnvironmentVariable("RESEND_API_KEY")
                ?? _configuration["Resend:ApiKey"] ?? "";
            var fromEmail = Environment.GetEnvironmentVariable("RESEND_FROM_EMAIL")
                ?? _configuration["Resend:FromEmail"] ?? "onboarding@resend.dev";

            if (string.IsNullOrEmpty(apiKey))
            {
                _logger.LogWarning("Resend not configured. Email to {Email} with subject '{Subject}' was not sent.", email, subject);
                return;
            }

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var payload = new
            {
                from = $"Top Notch Training <{fromEmail}>",
                to = new[] { email },
                subject,
                html = htmlMessage
            };

            var json = JsonSerializer.Serialize(payload);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await httpClient.PostAsync("https://api.resend.com/emails", content);
                var body = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Email sent to {Email} with subject '{Subject}'", email, subject);
                }
                else
                {
                    _logger.LogError("Resend failed ({StatusCode}): {Body}", response.StatusCode, body);
                    throw new Exception($"Resend error: {response.StatusCode} - {body}");
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
