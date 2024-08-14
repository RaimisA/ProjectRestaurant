using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectRestaurant.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectRestaurant.Services.Tests
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public void SendEmailTest()
        {
            // Arrange
            var emailService = new EmailService();
            var to = "test@example.com";
            var subject = "Test Subject";
            var body = "This is a test email body.";

            using (var sw = new StringWriter())
            {
                Console.SetOut(sw);

                // Act
                emailService.SendEmail(to, subject, body);

                // Assert
                var result = sw.ToString();
                Assert.IsTrue(result.Contains($"Sending email to {to}"), $"Expected to find 'Sending email to {to}' in the output. Actual output: {result}");
                Assert.IsTrue(result.Contains($"Subject: {subject}"), $"Expected to find 'Subject: {subject}' in the output. Actual output: {result}");
                Assert.IsTrue(result.Contains($"Body: {body}"), $"Expected to find 'Body: {body}' in the output. Actual output: {result}");
            }
        }
    }
}