using System.Threading;
using System.Threading.Tasks;
using ErrorConsole.Core.Interfaces;
using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ErrorConsole.Infrastructure.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions options)
            :base(options) { }

        public DbSet<StoredEvent> StoredEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
