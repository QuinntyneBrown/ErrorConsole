using ErrorConsole.Core.Models;
using ErrorConsole.Core.Extensions;
using ErrorConsole.Core.Identity;
using ErrorConsole.Infrastructure.Data;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System;
using Newtonsoft.Json;
using ErrorConsole.Core.DomainEvents;
using System.Threading;
using System.Security.Cryptography;

namespace ErrorConsole.API
{
    public class SeedData
    {
        public static void Seed(AppDbContext context)
        {
            UserConfiguration.Seed(context);

            CompanyConfiguration.Seed(context);
            context.SaveChanges();
        }

        internal class UserConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                var repository = new EventStoreRepository(context);

                if (context.StoredEvents
                    .Where(x => x.StreamId == new Guid("9f28229c-b39c-427e-8305-c1e07494d5d3"))
                    .FirstOrDefault() == null)
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
                    
                    repository.Save(user.UserId, user);
                }

                context.SaveChanges();
            }


        }

        internal class CompanyConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                var repository = new EventStoreRepository(context);

                if (repository.GetAllByEventProperyValue<CompanyCreated>("Name", "Ralph").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Save(aggregateId, new Company(aggregateId, "Ralph"));                    
                }

                if (repository.GetAllByEventProperyValue<CompanyCreated>("Name", "Kate Spade").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Save(aggregateId, new Company(aggregateId, "Kate Spade"));
                }

                if (repository.GetAllByEventProperyValue<CompanyCreated>("Name", "Nike").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Save(aggregateId, new Company(aggregateId, "Nike"));
                }
                

            }
        }
    }

    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddUserSecrets(typeof(Startup).GetTypeInfo().Assembly)
                .Build();

            return new AppDbContext(new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer(configuration["Data:DefaultConnection:ConnectionString"])
                .Options);
        }
    }
}
