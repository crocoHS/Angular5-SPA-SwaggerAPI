using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Todo.FunctionApp.SendEmail
{
    public static class SendGridEmail
    {
        static readonly string SendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");

        public static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY")
        };

        [FunctionName("Send")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequest req, ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string jsonContent = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            if (data.fromEmail == null || data.toEmail == null)
            {
                return new BadRequestObjectResult("Please pass fromEmail and toEmail in the request body");
            }

            string fromEmail = data.fromEmail.Value;
            string toEmail = data.toEmail.Value;
            string subject = data.subject.Value;
            string body = data.body.Value;
            bool isImportantEmail = bool.Parse(data.isImportant.Value.ToString());
            bool isBodyHtml = bool.Parse(data.isBodyHtml.Value.ToString());

            var message = new SendGridMessage
            {
                From = new EmailAddress(fromEmail),
                Subject = subject,
                Personalizations = new List<Personalization> {
                    new Personalization { Tos = new List<EmailAddress> { new EmailAddress(toEmail) } }
                },
                HtmlContent = $"{body}"
            };

            var client = new SendGridClient(SendGridApiKey);

            try
            {
                var startTime = DateTime.UtcNow;
                var timer = Stopwatch.StartNew();

                client.SendEmailAsync(message);
                telemetry.TrackDependency("SendEmail", "SendGrid", startTime, timer.Elapsed, true);
                log.LogInformation("Email successfully sent");
            }
            catch (Exception ex)
            {
                log.LogError($"Something went wrong: {ex.Message}");
                telemetry.TrackEvent("SendEmail Error");
                return new BadRequestObjectResult(false);
            }

            return new OkObjectResult(true);
        }
    }
}
