using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models
{
    public class EmailMessage
    {
        [Required]
        [JsonProperty(PropertyName = "fromEmail")]
        public string FromEmail { get; set; }

        [Required]
        [JsonProperty(PropertyName = "toEmail")]
        public string ToEmail { get; set; }

        [Required]
        [JsonProperty(PropertyName = "subject")]
        public string Subject { get; set; }

        [JsonProperty(PropertyName = "body")]
        public string Body { get; set; }

        [JsonProperty(PropertyName = "attachmentPath")]
        public string AttachmentPath { get; set; }

        [JsonProperty(PropertyName = "isBodyHtml")]
        public bool IsBodyHtml { get; set; }

        [JsonProperty(PropertyName = "isImportant")]
        public bool IsImportant { get; set; }
    }
}
