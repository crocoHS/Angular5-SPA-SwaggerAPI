using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Todo.Api.Models;
using Todo.Repository;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    public class BankController : Controller
    {
        [HttpGet("[action]")]
        public IActionResult GetAccountSummaryAll()
        {
            var dt = CustomerRepository.GetAccountSummaryAll();
            var accountList = GetAccountSummaryAll(dt);
            return new ObjectResult(accountList);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAccountSummary(int id)
        {
            var dt = CustomerRepository.GetAccountSummaryAll();
            var _accountSummaryList = GetAccountSummaryAll(dt)
                .Where(x => x.CustomerId == id);

            return new ObjectResult(_accountSummaryList);
        }

        [HttpGet("[action]/{id}")]
        public IActionResult GetAccountDetail(string id)
        {
            var dt = CustomerRepository.GetAccountSummaryAll();
            var accountSummaryList = GetAccountSummaryAll(dt);
            var summary = accountSummaryList.FirstOrDefault(a => a.AccountId.ToString() == id);
            if (summary == null)
                return NotFound();

            var random = new Random();

            IEnumerable<AccountTransaction> lstTransactions = Enumerable.Range(1, 10).Select(index => new AccountTransaction
            {
                TransactionDate = DateTimeOffset.Now - TimeSpan.FromDays(index),
                Description = $"Transaction #{index}",
                Amount = random.NextDouble() * 500 - 250
            });

            return new ObjectResult(new AccountDetail {
                CustomerId = summary.CustomerId,
                AccountSummary = summary,
                AccountTransactions = lstTransactions.ToArray() });
        }

        [HttpGet("[action]/{customerId}/{accountType}")]
        public IActionResult AddAccount(int customerId, int accountType)
        {
            var rnd = new Random();

            string accountNumber;
            string accountName = $"Account #{rnd.Next(1, 100)}";
            double balance = rnd.NextDouble() * 500 - 250;

            switch (accountType)
            {
                case 2:
                    accountNumber = $"{rnd.Next(1, 10000)}-{rnd.Next(1, 10000)}-{rnd.Next(1, 10000)}-{rnd.Next(1, 10000)}";
                    break;
                default:
                    accountNumber = $"{rnd.Next(1, 1000)}-{rnd.Next(1, 1000)}-{rnd.Next(1, 10000)}";
                    break;
            }

            var result = CustomerRepository.AddAccount(customerId, accountType, accountNumber, accountName, balance);

            return new ObjectResult(result);
        }

        [HttpGet("[action]/{accountId}")]
        public IActionResult DeleteAccount(int accountId)
        {
            var result = CustomerRepository.DeleteAccount(accountId);
            return new ObjectResult(result);
        }

        private List<AccountSummary> GetAccountSummaryAll(DataTable dt)
        {
            var accountList = new List<AccountSummary>();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var dr = dt.Rows[i];
                    var account = new AccountSummary
                    {
                        AccountId = Convert.ToInt32(dr["AccountId"]),
                        CustomerId = Convert.ToInt32(dr["CustomerId"]),
                        AccountName = dr["AccountName"].ToString(),
                        AccountNumber = dr["AccountNumber"].ToString(),
                        AccountType = GetAccountType(Convert.ToInt32(dr["AccountType"])),
                        Balance = Convert.ToDouble(dr["Balance"])
                    };

                    accountList.Add(account);
                }
            }

            return accountList;
        }

        private AccountType GetAccountType(int accountType)
        {
            switch (accountType)
            {
                case 0:
                    return AccountType.Checking;
                case 1:
                    return AccountType.Savings;
                default:
                    return AccountType.Credit;
            }
        }
    }
}
