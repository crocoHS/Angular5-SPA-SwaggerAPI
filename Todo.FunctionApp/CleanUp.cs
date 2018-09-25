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
        static readonly string OwnerId = Environment.GetEnvironmentVariable("TODO_OWNER_ID");

        // 0 */2 * * * *
        [FunctionName("CleanUp")]
        public static async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogDebug($"CleanUp Timer trigger function executed at: {DateTime.Now}");

            var options = new DbContextOptions<TodoContext>();

            using (var _context = new TodoContext(options))
            {
                await _context.Database.ExecuteSqlCommandAsync("DELETE FROM TodoItem WHERE IsComplete = 1 AND OwnerId = @OwnerId", new SqlParameter("@OwnerId", OwnerId));
            }

            log.LogDebug($"OwnerId {OwnerId} - Completed Todo Items have been deleted.");
            log.LogDebug($"CleanUp Timer trigger function finished at: {DateTime.Now}");
        }
    }
}
