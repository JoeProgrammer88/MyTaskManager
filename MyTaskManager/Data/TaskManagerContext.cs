using Microsoft.EntityFrameworkCore;
using MyTaskManager.Models;

namespace MyTaskManager.Data
{
    public class TaskManagerContext : DbContext
    {
        public TaskManagerContext(DbContextOptions<TaskManagerContext> options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Filename=MyTaskManager.db;");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<ToDoItem>(todo =>
            {
                todo.HasKey(x => x.Id);                         // PK
                todo.Property(x => x.Id).ValueGeneratedOnAdd(); // Identity column
            });
        }

        public DbSet<ToDoItem> ToDos { get; set; }
    }
}
