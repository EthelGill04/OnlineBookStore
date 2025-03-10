using Hangfire;
using Hangfire.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BookStoreReminderAPI.Controllers;
using BookStoreReminderAPI.Models;
using Microsoft.Identity.Client;
using BookStoreReminderAPI.Services;
namespace BookStoreReminderAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowWebApp",
                    policy => policy.WithOrigins("https://localhost:7252")
                                    .AllowAnyHeader()
                                    .AllowAnyMethod());
            });

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();


            // Add Database Context
            builder.Services.AddDbContext<OnlineBookStoreDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Configure Hangfire
            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddHangfireServer();

            // Register Email Service
            builder.Services.AddScoped<EmailService>();

            // Register the Background Service
            builder.Services.AddHostedService<EmailReminderService>();

            builder.Services.AddScoped<BookService>();

            var app = builder.Build();

            app.UseHangfireDashboard(); // Optional: Enables Hangfire Dashboard at /hangfire
            app.UseHangfireServer();

            app.UseCors("AllowWebApp");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Schedule Hangfire Job on Application Start
            RecurringJob.AddOrUpdate<EmailReminderService>(
                "send-email-reminders",
                job => job.SendRemindersAsync(), Cron.Daily); // Runs every day

            app.Run();
        }
    }
}
