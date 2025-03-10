using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;

namespace BookStoreReminderAPI.Controllers
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendReminderEmailAsync(string email, string bookTitle, DateTime dueDate)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var smtpUsername = _configuration["EmailSettings:SmtpUsername"];
            var smtpPassword = _configuration["EmailSettings:SmtpPassword"];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Online Book Store", smtpUsername));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Book Return Reminder";
            message.Body = new TextPart("plain")
            {
                Text = $"Reminder: The book '{bookTitle}' is due on {dueDate:yyyy-MM-dd}. Please return it on time."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(smtpServer, smtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtpUsername, smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        internal async Task SendEmailAsync(string borrowerEmail, string subject, string message)
        {
            throw new NotImplementedException();
        }
    }

}
