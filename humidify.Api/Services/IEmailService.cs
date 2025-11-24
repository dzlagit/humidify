namespace humidify.Api.Services
{
    public interface IEmailService // <-- MUST BE SINGULAR
    {
        Task SendEmailAsync(string userEmail, string subject, string body);
    }
}