using ErrorConsole.Core.Identity;
using ErrorConsole.Core.Models;
using ErrorConsole.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Security.Cryptography;

namespace ErrorConsole.API
{
    public class AppInitializer: IDesignTimeDbContextFactory<AppDbContext>
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);
            CompanyConfiguration.Seed(context);
            context.SaveChanges();
        }

        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly)
                .Build();

            return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
                .Options);
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                var eventStore = new EventStore(context);

                if (eventStore.Query<User>("Username", "quinntynebrown@gmail.com") == null)
                {
                    var salt = new byte[128 / 8];
                    using (var rng = RandomNumberGenerator.Create())
                    {
                        rng.GetBytes(salt);
                    }
                    
                    var user = new User(new Guid("9f28229c-b39c-427e-8305-c1e07494d5d3"),
                        "quinntynebrown@gmail.com",
                        salt,
                        new PasswordHasher().HashPassword(salt, "P@ssw0rd")
                        );
                    
                    eventStore.Save(user);
                }

                context.SaveChanges();
            }
        }

        internal class CompanyConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                var eventStore = new EventStore(context);

                if (eventStore.Query<Company>("Name", "Ralph") == null)
                    eventStore.Save(new Company(Guid.NewGuid(), "Ralph"));                    


                if (eventStore.Query<Company>("Name", "Kate Spade") == null)
                    eventStore.Save(new Company(Guid.NewGuid(), "Kate Spade"));


                if (eventStore.Query<Company>("Name", "Nike") == null)
                    eventStore.Save(new Company(Guid.NewGuid(), "Nike"));
            }
        }
    }
    
}
