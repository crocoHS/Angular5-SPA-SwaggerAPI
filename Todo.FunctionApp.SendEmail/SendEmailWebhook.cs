
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Todo.FunctionApp.SendEmail
{
    public static class SendEmailWebhook
    {
        static readonly string GmailCredentialUsername = Environment.GetEnvironmentVariable("GMAIL_CREDENTIALUSERNAME");
        static readonly string GmailCredentialPassword = Environment.GetEnvironmentVariable("GMAIL_CREDENTIALPASSWORD");
        static readonly string GmailHost = Environment.GetEnvironmentVariable("GMAIL_HOST");
        static readonly int GmailPort = Convert.ToInt32(Environment.GetEnvironmentVariable("GMAIL_PORT"));

        public static TelemetryClient telemetry = new TelemetryClient()
        {
            InstrumentationKey = Environment.GetEnvironmentVariable("APPINSIGHTS_INSTRUMENTATIONKEY")
        };

        [FunctionName("SendEmailWebhook")]
        public static async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequest req, TraceWriter log)
        {
            log.Info($"SendEmail Webhook is triggered!");

            string jsonContent = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            if (data.fromEmail == null || data.toEmail == null)
            {
                return new BadRequestObjectResult("Please pass fromEmail and toEmail in the request body");
            }

            string fromEmail = data.fromEmail;
            string toEmail = data.toEmail;
            string subject = data.subject;
            string body = data.body;
            string attachmentPath = data.attachmentPath;
            bool isImportantEmail = bool.Parse(data.isImportant.ToString());
            bool isBodyHtml = bool.Parse(data.isBodyHtml.ToString());

            SmtpClient smtp = new SmtpClient(GmailHost, GmailPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(GmailCredentialUsername, GmailCredentialPassword)
            };

            MailAddress maFrom = new MailAddress(fromEmail);
            MailAddress maTo = new MailAddress(toEmail);
            MailMessage mailMessage = new MailMessage(maFrom, maTo)
            {
                Subject = subject,
                IsBodyHtml = isBodyHtml,
                Body = $"{body}\nSent from:{fromEmail}",
                Priority = isImportantEmail ? MailPriority.High : MailPriority.Normal
            };

            if (!String.IsNullOrEmpty(attachmentPath))
            {
                mailMessage.Attachments.Add(new Attachment(attachmentPath));
            }

            try
            {
                var startTime = DateTime.UtcNow;
                var timer = Stopwatch.StartNew();

                // Allow less secure apps to be turned ON: https://myaccount.google.com/lesssecureapps
                await smtp.SendMailAsync(mailMessage);

                telemetry.TrackDependency("SendEmailWebhook", "SendMailAsync", startTime, timer.Elapsed, true);
                log.Info("Email successfully sent");

                return new OkObjectResult(true);
            }

            catch (Exception ex)
            {
                log.Error(ex.ToString());
                return new BadRequestObjectResult($"Something went wrong: {ex.Message}");
            }
        }
    }
}
