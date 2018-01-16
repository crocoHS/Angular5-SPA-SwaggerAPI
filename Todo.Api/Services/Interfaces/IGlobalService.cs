using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Api.Models;

namespace Todo.Api.Services.Interfaces
{
    public interface IGlobalService
    {
        Task<bool> SendMailAsync(EmailMessage emailMessage);
        Task<bool> SendEmailWebhook(EmailMessage emailMessage, string functionUri);
        Task<string> GetConsumersAsync();
        IEnumerable<WeatherForecast> GetWeatherForecastsAsync();
    }
}