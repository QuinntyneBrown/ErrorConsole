using ErrorConsole.Core.Extensions;
using ErrorConsole.Core.Identity;
using ErrorConsole.Core.Middleware;
using ErrorConsole.Infrastructure.Extensions;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorConsole.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
            => Configuration = configuration;
        
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCustomMvc()
                .AddFluentValidation(cfg => { cfg.RegisterValidatorsFromAssemblyContaining<Startup>(); });

            services
                .AddCustomSecurity(Configuration)
                .AddCustomSwagger()
                .AddDataStore(Configuration["Data:DefaultConnection:ConnectionString"])
                .AddMediatR(typeof(Startup).Assembly);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLoggerMiddleware>();

            if(Configuration.GetValue<bool>("isTest"))
                app.UseMiddleware<AutoAuthenticationMiddleware>();
                    
            app.UseAuthentication()            
                .UseCors(CorsDefaults.Policy)            
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "ErrorConsole API");
                    options.RoutePrefix = string.Empty;
                });
        }        
    }


}
