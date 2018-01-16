using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;
using Todo.Core;
using Todo.Model;

namespace Todo.Api.Services
{
    public class GlobalService : IGlobalService
    {
        readonly List<string> _forcasts = new List<string>
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public IEnumerable<WeatherForecast> GetWeatherForecastsAsync()
        {
            var rng = new Random();

            var lstWeatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateCast = Utilities.GetRandomDate(DateTime.Now, DateTime.Now.AddYears(1)),
                TemperatureC = rng.Next(-20, 55),
                Summary = _forcasts[rng.Next(_forcasts.Count)]
            });

            return lstWeatherForecast;
        }

        public async Task<string> GetConsumersAsync()
        {
            using (var httpClient = new HttpClient())
            {
                return await httpClient.GetStringAsync("http://jsonplaceholder.typicode.com/users");
            }
        }

        public async Task<bool> SendMailAsync(EmailMessage emailMessage)
        {
            if (string.IsNullOrEmpty(emailMessage.FromEmail) || string.IsNullOrEmpty(emailMessage.ToEmail))
            {
                throw new InvalidOperationException("Email From and To fields cannot be empty");
            }

            var email = Mapper.Map<EmailMessage, EmailMessageDT>(emailMessage);
            bool isSent = await MailSender.SendMailAsync(email);

            return isSent;
        }

        public async Task<bool> SendEmailWebhook(EmailMessage emailMessage, string functionUri)
        {
            if (string.IsNullOrEmpty(emailMessage.FromEmail) || string.IsNullOrEmpty(emailMessage.ToEmail))
            {
                throw new InvalidOperationException("Email From and To fields cannot be empty");
            }

            var email = Mapper.Map<EmailMessage, EmailMessageDT>(emailMessage);
            bool isSuccess = await MailSender.SendEmailWebhook(email, functionUri);

            return isSuccess;
        }
    }
}
