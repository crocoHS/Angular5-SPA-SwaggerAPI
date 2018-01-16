using System;

namespace Todo.Api.Models
{
    public class WeatherForecast
    {
        public DateTime DateCast{ get; set; }
        public int TemperatureC { get; set; }
        public string Summary { get; set; }
        public int TemperatureF
        {
            get
            {
                return 32 + (int)(TemperatureC / 0.5556);
            }
        }
    }
}