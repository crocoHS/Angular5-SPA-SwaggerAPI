using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Todo.FunctionApp
{
    public static class CleanUp
    {
        static readonly string itemName = Environment.GetEnvironmentVariable("TODO_ITEM_NAME");

        // 0 */2 * * * *
        [FunctionName("CleanUp")]
        public static async Task Run([TimerTrigger("0 10 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogDebug($"CleanUp Timer trigger function executed at: {DateTime.Now}");

            var options = new DbContextOptions<TodoContext>();

            using (var _context = new TodoContext(options))
            {
                await _context.Database.ExecuteSqlCommandAsync("DELETE FROM TodoItem WHERE Name = @Name AND IsComplete = 1", new SqlParameter("@Name", itemName));
            }

            log.LogDebug($"CleanUp Timer trigger function finished at: {DateTime.Now}");
        }
    }
}
