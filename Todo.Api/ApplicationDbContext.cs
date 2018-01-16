using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo.Model;

namespace Todo.Api
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

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
    }
}
