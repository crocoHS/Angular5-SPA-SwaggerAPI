using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Todo.Model;
using Todo.Repository;

namespace Todo.FunctionApp.SendEmailWebhook
{
    public static class BlobFileDrop
    {
        [FunctionName("CsvFileProcess")]
        public static async Task Run([BlobTrigger("import/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {
            log.LogDebug($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            if (myBlob.Length == 0)
            {
                log.LogError("The CSV file is empty");
                return;
            }

            var json = await CsvToJsonAsync(myBlob);

            if (string.IsNullOrEmpty(json))
            {
                log.LogError($"Something went wrong when trying to convert CSV to JSON");
                return;
            }

            var todoItems = json.FromJson<List<TodoItem>>();
            var count = await ProcessTodoItemsAsync(todoItems, name, log);

            log.LogDebug($"{count} todo item(s) have been inserted to the DB");
        }

        private static async Task<string> CsvToJsonAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var content = await reader.ReadToEndAsync();
                return content.CsvToJson();
            }
        }

        private static async Task<int> ProcessTodoItemsAsync(List<TodoItem> todoItems, string name, ILogger log)
        {
            var options = new DbContextOptions<TodoContext>();

            using (var context = new TodoContext(options))
            {
                foreach (var item in todoItems)
                {
                    if (context.TodoItems.Any(t => t.Name == item.Name && t.OwnerId == item.OwnerId))
                    {
                        log.LogWarning($"{name}: Duplicate item name: \"{item.Name}\".");
                    }

                    else
                    {
                        context.TodoItems.Add(item);
                        log.LogDebug($"{name}: Inserted name: \"{item.Name}\" with ID: {item.Id}.");
                    }
                }

                return await context.SaveChangesAsync();
            }
        }
    }
}
