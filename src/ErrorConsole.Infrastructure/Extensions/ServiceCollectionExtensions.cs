using ErrorConsole.Core.Interfaces;
using ErrorConsole.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorConsole.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {                
        public static IServiceCollection AddDataStore(this IServiceCollection services,
                                               string connectionString)
        {
            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped<IEventStore, EventStore>();

            return services.AddDbContext<AppDbContext>(options 
                => options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ErrorConsole.Infrastructure")));          
        }
    }
}
