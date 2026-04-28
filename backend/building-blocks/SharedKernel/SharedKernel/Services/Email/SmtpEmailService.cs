using Microsoft.Extensions.Logging;

namespace SharedKernel.Services.Email
{
    public class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;

        public SmtpEmailService(ILogger<SmtpEmailService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string to, string subject, string body)
        {
            _logger.LogInformation(
                "Sending email (mock). To: {To}, Subject: {Subject}, Body: {Body}",
                to,
                subject,
                body);

            return Task.CompletedTask;
        }
    }
}
