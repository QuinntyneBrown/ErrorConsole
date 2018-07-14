using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<DomainEvent> DomainEvents { get; set; }
        DbSet<Company> Companies { get; set; }                
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
