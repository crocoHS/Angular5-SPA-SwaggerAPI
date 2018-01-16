using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using Todo.Api.Models;
using Todo.Api.Services.Interfaces;
using Todo.Core;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    public class GlobalController : Controller
    {
        readonly IDataProtector _protector;
        readonly IGlobalService _globalService;
        readonly IHostingEnvironment _env;
        readonly IConfiguration _configuration;

        public GlobalController(IGlobalService globalService, 
            IDataProtectionProvider provider, 
            IHostingEnvironment env, 
            IConfiguration configuration)
        {
            _protector = provider.CreateProtector(GetType().FullName);
            _globalService = globalService;
            _env = env;
            _configuration = configuration;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetConsumersAsync()
        {
            var json = await _globalService.GetConsumersAsync();
            return Ok(json);
        }

        [HttpGet("[action]")]
        public IActionResult WeatherForecasts()
        {
            var lstWeatherForecast = _globalService.GetWeatherForecastsAsync();
            return new ObjectResult(lstWeatherForecast);
        }

        [HttpPost("[action]")]
        public IActionResult Encrypt([FromBody]PlainText plainText)
        {
            try
            {
                var result = new PlainText() { Text = _protector.Protect(plainText.Text) };
                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Something went wrong: {ex.Message}");
            }

        }

        [HttpPost("[action]")]
        public IActionResult Decrypt([FromBody]PlainText plainText)
        {
            try
            {
                var result = new PlainText() { Text = _protector.Unprotect(plainText.Text) };
                return new ObjectResult(result);
            }
            catch (Exception ex)
            {
                return new ObjectResult($"Something went wrong: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMailAsync([FromBody] EmailMessage emailMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            bool result = await _globalService.SendMailAsync(emailMessage);

            return Ok(result);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendEmailWebhook([FromBody] EmailMessage emailMessage)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var functionUri = _env.IsDevelopment() ?
                Constants.AzureFunctionUrl : 
                $"{Constants.AzureFunctionApi}/SendEmailWebhook";

            bool result = await _globalService.SendEmailWebhook(emailMessage, functionUri);

            return Ok(result);
        }
    }
}
