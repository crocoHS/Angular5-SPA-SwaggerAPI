using Newtonsoft.Json;

namespace Todo.FunctionApp.SendEmailWebhook
{
    public class Personalization : SendGrid.Helpers.Mail.Personalization
    {
        [JsonProperty("dynamic_template_data")]
        public dynamic TemplateData { get; set; }
    }
}
