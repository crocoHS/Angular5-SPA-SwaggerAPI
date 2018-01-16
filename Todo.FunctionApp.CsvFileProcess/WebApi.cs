using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Todo.FunctionApp.CsvFileProcess;
using Todo.Model;
using Todo.Repository;

namespace Todo.FunctionApp.FileProcess
{
    public static class WebApi
    {
        [FunctionName("CsvFileProcess")]
        public static async Task Run([BlobTrigger("import/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, TraceWriter log)
        {
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");

            if (myBlob.Length == 0)
            {
                log.Error("The CSV file is empty");
                return;
            }

            var json = await CsvToJsonAsync(myBlob);

            if (String.IsNullOrEmpty(json))
            {
                log.Error($"Something went wrong when trying to convert CSV to JSON");
                return;
            }

            var todoItems = json.FromJson<List<TodoItem>>();
            var count = await ProcessTodoItemsAsync(todoItems, name, log);

            log.Info($"{count} todo item(s) have been inserted to the DB");
        }

        private static async Task<string> CsvToJsonAsync(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var content = await reader.ReadToEndAsync();
                return content.CsvToJson();
            }
        }

        private static async Task<int> ProcessTodoItemsAsync(List<TodoItem> todoItems, string name, TraceWriter log)
        {
            var options = new DbContextOptions<TodoContext>();

            using (var context = new TodoContext(options))
            {
                foreach (var item in todoItems)
                {
                    if (context.TodoItems.Any(t => t.Name == item.Name && t.OwnerId == item.OwnerId))
                    {
                        log.Warning($"{name}: Duplicate item name: \"{item.Name}\".");
                    }

                    else
                    {
                        context.TodoItems.Add(item);
                        log.Info($"{name}: Inserted name: \"{item.Name}\" with ID: {item.Id}.");
                    }
                }

                return await context.SaveChangesAsync();
            }
        }
    }
}
