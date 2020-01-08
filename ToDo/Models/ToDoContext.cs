using System.Data.Entity;

namespace ToDo.Models
{
    public class ToDoContext : DbContext
    {
        public ToDoContext():base("ToDoDb")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ToDoContext,
               ToDo.Migrations.Configuration>("ToDoDb"));
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<CustomList> CustomLists { get; set; }
        public DbSet<SmartList> SmartLists { get; set; }
        
    }
}