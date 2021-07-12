using Microsoft.EntityFrameworkCore;

namespace TelcoTestAsp.Models
{
    public class TelcoContext : DbContext
    {
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskElement> TaskElements { get; set; }

        public TelcoContext(DbContextOptions<TelcoContext> options)
           : base(options)
        {
            //Database.EnsureCreated();
        }
    }
}
