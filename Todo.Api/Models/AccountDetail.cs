namespace Todo.Api.Models
{
    public class AccountDetail
    {
        public int CustomerId { get; set; }
        public AccountSummary AccountSummary { get; set; }
        public AccountTransaction[] AccountTransactions { get; set; }
    }
}