using Newtonsoft.Json;

namespace Todo.Api.Models
{
    public class AccountSummary
    {
        [JsonProperty(PropertyName = "accountId")]
        public int AccountId { get; set; }

        [JsonProperty(PropertyName = "customerId")]
        public int CustomerId { get; set; }

        [JsonProperty(PropertyName = "accountNumber")]
        public string AccountNumber { get; set; }

        [JsonProperty(PropertyName = "accountType")]
        public AccountType AccountType { get; set; }

        [JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        [JsonProperty(PropertyName = "balance")]
        public double Balance { get; set; }
    }
}