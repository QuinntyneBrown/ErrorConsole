using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ErrorConsole.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options) { }

        public DbSet<DomainEvent> DomainEvents { get; set; }
        public DbSet<Company> Companies { get; set; }        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }       
    }
}
