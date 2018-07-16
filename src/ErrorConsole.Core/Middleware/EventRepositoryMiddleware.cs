using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorConsole.Core.Middleware
{
    public static class EventRepositoryMiddleware
    {
        public static IApplicationBuilder UseAppService(this IApplicationBuilder app)
        {
            var services = (IServiceScopeFactory)app.ApplicationServices.GetService(typeof(IServiceScopeFactory));

            
            return app;
        }
    }
}
