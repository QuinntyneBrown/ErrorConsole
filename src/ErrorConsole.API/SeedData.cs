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
                    var userId = new Guid("9f28229c-b39c-427e-8305-c1e07494d5d3");

                    var user = new User()
                    {
                        Username = "quinntynebrown@gmail.com"
                    };

                    user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                    var userCreatedEvent = new UserCreatedEvent()
                    {
                        UserId = userId,
                        Username = "quinntynebrown@gmail.com",
                        Salt = user.Salt,
                        Password = user.Password
                    };

                    repository.Store(userId, userCreatedEvent);
                }

                context.SaveChanges();
            }


        }

        internal class CompanyConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                var repository = new EventStoreRepository(context);

                if (repository.GetAllByEventProperyValue<CompanyCreatedEvent>("Name", "Ralph").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Store<CompanyCreatedEvent>(aggregateId, new CompanyCreatedEvent()
                    {
                        CompanyId = aggregateId,
                        Name = "Ralph"
                    });
                }

                if (repository.GetAllByEventProperyValue<CompanyCreatedEvent>("Name", "Kate Spade").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Store(aggregateId, new CompanyCreatedEvent()
                    {
                        CompanyId = aggregateId,
                        Name = "Kate Spade"
                    });
                }

                if (repository.GetAllByEventProperyValue<CompanyCreatedEvent>("Name", "Nike").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Store(aggregateId, new CompanyCreatedEvent()
                    {
                        CompanyId = aggregateId,
                        Name = "Nike"
                    });
                }

                if (repository.GetAllByEventProperyValue<CompanyCreatedEvent>("Name", "Nike").FirstOrDefault() == null)
                {
                    var aggregateId = Guid.NewGuid();
                    repository.Store(aggregateId, new CompanyCreatedEvent()
                    {
                        CompanyId = aggregateId,
                        Name = "Nike"
                    });
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
