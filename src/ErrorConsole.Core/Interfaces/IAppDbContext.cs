using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ErrorConsole.Core.Interfaces
{
    public interface IAppDbContext: IDisposable
    {
        DbSet<StoredEvent> StoredEvents { get; set; }                 
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        int SaveChanges();
    }
}
