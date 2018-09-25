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

namespace Todo.FunctionApp
{
    public static class SendGridEmail
    {
        static readonly string InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY");
        static readonly string SendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_APIKEY");

        public static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = InstrumentationKey
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

            if (data.templateId == null)
            {
                return new BadRequestObjectResult("Please pass templateId in the request body");
            }

            string templateId = data.templateId.Value;
            string fromEmail = data.fromEmail.Value;
            string fromName = data.fromName.Value;
            string toEmail = data.toEmail.Value;
            string toName = data.customerId.Value;

            var message = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                TemplateId = templateId,
                Personalizations = new List<SendGrid.Helpers.Mail.Personalization> {
                    new Personalization {
                        Tos = new List<EmailAddress> { new EmailAddress(toEmail, toName) },
                        TemplateData = data
                    }
                }
            };

            var client = new SendGridClient(SendGridApiKey);

            try
            {
                var startTime = DateTime.UtcNow;
                var timer = Stopwatch.StartNew();

                client.SendEmailAsync(message);
                telemetry.TrackDependency("SendGrid", "Email", "Send", startTime, timer.Elapsed, true);
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
