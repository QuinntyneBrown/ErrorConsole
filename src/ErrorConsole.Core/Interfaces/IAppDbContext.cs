using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Core.Interfaces
{
    public interface IAppDbContext
    {
        DbSet<Company> Companies { get; set; }
        DbSet<User> Users { get; set; }        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
