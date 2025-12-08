namespace humidify.Api.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string userEmail, string subject, string body);
    }
}