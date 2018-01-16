using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Todo.Model;

namespace Todo.FunctionApp.PostTodo
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("local.settings.json", optional: true)
            .AddEnvironmentVariables();

            var config = builder.Build();

            var connection = config.GetConnectionString("TodoContext");
            optionsBuilder.UseSqlServer(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TodoItem>()
            .ToTable("TodoItem");

            builder.Entity<TodoItem>()
            .Property(b => b.Id)
            .ValueGeneratedOnAdd();

            builder.Entity<TodoItem>()
            .HasKey(b => b.Id);

            base.OnModelCreating(builder);
        }

        public DbSet<TodoItem> TodoItems { get; set; }
    }
}