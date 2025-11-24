using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace humidify.Api.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _senderEmail;
        private readonly string _supportEmail;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGridSettings:ApiKey"]
                      ?? throw new InvalidOperationException("SendGridSettings:ApiKey not configured in appsettings.json.");

            _senderEmail = configuration["SendGridSettings:SenderEmail"]
                           ?? throw new InvalidOperationException("SendGridSettings:SenderEmail not configured in appsettings.json.");

            _supportEmail = configuration["SendGridSettings:SupportEmail"]
                            ?? throw new InvalidOperationException("SendGridSettings:SupportEmail not configured in appsettings.json.");
        }

        public async Task SendEmailAsync(string userEmail, string subject, string body)
        {
            var client = new SendGridClient(_apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_senderEmail, "Humidify Contact Form"),
                Subject = subject,
                PlainTextContent = body,
                HtmlContent = body.Replace("\n", "<br/>")
            };

            msg.SetReplyTo(new EmailAddress(userEmail));

            msg.AddTo(new EmailAddress(_supportEmail));

            var response = await client.SendEmailAsync(msg);

            if (response.IsSuccessStatusCode)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n--- EMAIL SENT VIA SENDGRID (Status: {response.StatusCode}) ---");
                Console.WriteLine($"To (Support): {_supportEmail}");
                Console.WriteLine($"Reply-To (User): {userEmail}");
                Console.WriteLine("-------------------------------------------------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                var responseBody = await response.Body.ReadAsStringAsync();
                Console.WriteLine($"\n--- SENDGRID FAILURE (Status: {response.StatusCode}) ---");
                Console.WriteLine($"Error Body: {responseBody}");
                Console.WriteLine("-------------------------------------------------");
            }
            Console.ResetColor();
        }
    }
}