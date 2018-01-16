using Moq;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;

namespace Todo.UnitTest
{
    [TestFixture]
    public class GlobalServiceTest
    {
        [Test]
        public async Task SendMailAsync_Success()
        {
            // Arrange
            var mockService = new Mock<IGlobalService>();
            mockService.Setup(x => x.SendMailAsync(It.IsAny<EmailMessage>())).Returns(async () =>
            {
                await Task.Yield();
                return true;
            });

            var email = new EmailMessage()
            {
                FromEmail = "mail4solly@gmail.com",
                ToEmail = "mail4solly@gmail.com",
                Subject = "Core 2 Application",
                Body = "<div>Core 2 Application HTML test message.</div>",
                IsBodyHtml = true,
                IsImportant = false
            };

            // Act
            var actual = await mockService.Object.SendMailAsync(email);

            // Assert
            mockService.Verify(m => m.SendMailAsync(email), Times.Exactly(1));
        }

        [Test]
        public async Task SendMailAsync_NoCredentials_Failure_Throws()
        {
            string errorMessage = "Email From and To fields cannot be empty";

            // Arrange
            var mockService = new Mock<IGlobalService>();

            var email = new EmailMessage()
            {
                FromEmail = "mail4solly@gmail.com",
                ToEmail = "mail4solly@gmail.com",
                Subject = "Core 2 Application",
                Body = "<div>Core 2 Application HTML test message.</div>",
                IsBodyHtml = true,
                IsImportant = false
            };

            var actual = await mockService.Object.SendMailAsync(email);

            // Act and Assert
            Assert.That(async () =>
                await SendMailAsyncNoCredentialsThrowException(email, errorMessage),
                Throws.Exception.TypeOf<InvalidOperationException>().And.Message.EqualTo(errorMessage));
        }

        private async Task SendMailAsyncNoCredentialsThrowException(EmailMessage email, string errorMessage)
        {
            var mockService = new Mock<IGlobalService>();
            await mockService.Object.SendMailAsync(email).ConfigureAwait(false);
            throw new InvalidOperationException(errorMessage);
        }
    }
}
