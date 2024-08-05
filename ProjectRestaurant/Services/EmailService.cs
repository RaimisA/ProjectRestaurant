using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectRestaurant.Services.Interfaces;

namespace ProjectRestaurant.Services
{
    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body)
        {
            // Implementation for sending email
        }
    }
}
