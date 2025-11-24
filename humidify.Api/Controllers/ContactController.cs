using humidify.Core.Models;
using humidify.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace humidify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public ContactController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostContactMessage([FromBody] ContactMessage message)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var subject = $"New Contact Form Submission from {message.Name}";
            var body = $"""
                Name: {message.Name}
                Email: {message.Email}
                
                Message:
                {message.Message}
            """;

            await _emailService.SendEmailAsync(
                userEmail: message.Email,
                subject: subject,
                body: body
            );

            return Ok(new { status = "Message received and email sent via SendGrid." });
        }
    }
}