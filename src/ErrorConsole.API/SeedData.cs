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
                
                var user = new User()
                {
                    Username = "quinntynebrown@gmail.com"
                };

                user.Password = new PasswordHasher().HashPassword(user.Salt, "P@ssw0rd");

                context.DomainEvents.Add(new DomainEvent()
                {
                    Data = JsonConvert.SerializeObject(new UserCreatedEvent()
                    {
                        UserId = Guid.NewGuid(),
                        Username = "quinntynebrown@gmail.com",
                        Salt = user.Salt,
                        Password = user.Password

                    }),
                    Type = nameof(UserCreatedEvent),
                    DotNetType = $"{typeof(UserCreatedEvent).AssemblyQualifiedName}",
                    CreatedOn = DateTime.UtcNow
                });

            
                context.SaveChanges();
            }


        }

        internal class CompanyConfiguration
        {
            public static void Seed(AppDbContext context)
            {
                if (context.Companies.FirstOrDefault(x => x.Name == "Nike") == null)
                {
                    var nike = new Company()
                    {
                        Name = "Nike"
                    };

                    var ralph = new Company()
                    {
                        Name = "Ralph Lauren"
                    };

                    var kate = new Company()
                    {
                        Name = "Kate Spade"
                    };


                    context.Companies.AddRange(new[] { nike, ralph, kate });
                }

                context.SaveChanges();
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
