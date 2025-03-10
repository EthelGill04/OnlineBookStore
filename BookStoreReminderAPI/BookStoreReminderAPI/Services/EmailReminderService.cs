using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MailKit.Net.Smtp;
using MimeKit;
using BookStoreReminderAPI.Controllers;
using BookStoreReminderAPI.Models;
namespace BookStoreReminderAPI.Services;

public class EmailReminderService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<EmailReminderService> _logger;
    private readonly IConfiguration _configuration;

    public EmailReminderService(IServiceProvider serviceProvider, ILogger<EmailReminderService> logger, IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Email Reminder Service started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SendRemindersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Email Reminder Service: {ex.Message}");
            }

            // Wait for 24 hours before running again (86400000 ms = 24 hours)
            await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
        }
    }

    public async Task SendRemindersAsync()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<OnlineBookStoreDbContext>();
            var emailService = scope.ServiceProvider.GetRequiredService<EmailService>();

            var today = DateTime.UtcNow.Date;
            var dueSoon = dbContext.BorrowedBooks
                .Where(b => b.DueDate.Date == today.AddDays(3) && b.ReturnDate == null)
                .ToList();

            foreach (var book in dueSoon)
            {
                string subject = "Book Return Reminder";
                string message = $"Hello, your borrowed book '{book.Title}' is due on {book.DueDate:yyyy-MM-dd}. Please return it on time.";

                _logger.LogInformation($"Sending email reminder to {book.BorrowerEmail} for book '{book.Title}' due on {book.DueDate}");

                await emailService.SendEmailAsync(book.BorrowerEmail, subject, message);
            }
        }
    }
}
