using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Todo.Api.Models
{
    public class PlainText
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required]
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
    }
}