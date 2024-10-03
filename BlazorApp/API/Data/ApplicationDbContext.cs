using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Employee> employees { get; set; }
        public DbSet<Objects> objects { get; set; }
        public DbSet<Work> work { get; set; }
    }
}
