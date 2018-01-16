using System;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Todo.Model;
using Todo.Repository;

namespace Todo.Core
{
    public class MailSender
    {
        public static async Task<bool> SendMailAsync(EmailMessageDT emailMessage)
        {
            SmtpClient smtp = new SmtpClient(Constants.GmailHost, Constants.GmailPort)
            {
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(Constants.GmailCredentialUsername, Constants.GmailCredentialPassword)
            };

            MailAddress maFrom = new MailAddress(emailMessage.FromEmail);
            MailAddress maTo = new MailAddress(emailMessage.ToEmail);
            MailMessage mailMessage = new MailMessage(maFrom, maTo)
            {
                Subject = emailMessage.Subject,
                IsBodyHtml = emailMessage.IsBodyHtml,
                Body = $"{emailMessage.Body}\nSent from:{emailMessage.FromEmail}",
                Priority = emailMessage.IsImportant ? MailPriority.High : MailPriority.Normal
            };

            if (!String.IsNullOrEmpty(emailMessage.AttachmentPath))
            {
                mailMessage.Attachments.Add(new Attachment(emailMessage.AttachmentPath));
            }

            bool isSent;

            try
            {
                // Allow less secure apps to be turned ON: https://myaccount.google.com/lesssecureapps
                await smtp.SendMailAsync(mailMessage);
                isSent = true;
            }
            catch (Exception)
            {
                isSent = false;
            }

            return isSent;
        }

        public static async Task<bool> SendEmailWebhook(EmailMessageDT emailMessage, string functionUri)
        {
            var json = emailMessage.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(functionUri, content);
                return response.IsSuccessStatusCode;
            }
        }
    }
}
