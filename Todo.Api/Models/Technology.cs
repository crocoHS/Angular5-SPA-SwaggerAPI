using Newtonsoft.Json;

namespace Todo.Api.Models
{
    public class Technology
    {
        [JsonProperty(PropertyName = "technologyId")]
        public int TechnologyId { get; set; }

        [JsonProperty(PropertyName = "technologyName")]
        public string TechnologyName { get; set; }
    }
}
