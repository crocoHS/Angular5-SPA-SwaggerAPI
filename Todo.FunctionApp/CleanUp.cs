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
        // 0 */2 * * * *
        [FunctionName("CleanUp")]
        public static async Task Run([TimerTrigger("0 10 * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogDebug($"CleanUp Timer trigger function executed at: {DateTime.Now}");

            var options = new DbContextOptions<TodoContext>();

            using (var _context = new TodoContext(options))
            {
                string ownerId = "117679367306256560152";
                await _context.Database.ExecuteSqlCommandAsync("DELETE FROM TodoItem WHERE IsComplete = 1 AND OwnerId = @OwnerId", new SqlParameter("@OwnerId", ownerId));
            }

            log.LogDebug($"CleanUp Timer trigger function finished at: {DateTime.Now}");
        }
    }
}
