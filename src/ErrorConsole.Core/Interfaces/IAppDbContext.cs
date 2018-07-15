using ErrorConsole.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ErrorConsole.Core.Interfaces
{
    public interface IAppDbContext: IDisposable
    {
        DbSet<StoredEvent> StoredEvents { get; set; }                 
        int SaveChanges();
    }
}
