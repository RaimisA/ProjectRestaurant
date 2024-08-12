using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;
using ProjectRestaurant.Services.Interfaces;

namespace ProjectRestaurant.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // Simulation of sending an email
            Console.BackgroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"Sending email to {to}");
            Console.ResetColor();
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body: {body}");
        }
    }
}
