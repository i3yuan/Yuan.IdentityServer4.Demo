using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yuan.IdentityServer4.EF
{
    public class SeedData
    {
        public static void EnsureSeedData(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding database...");

            using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<PersistedGrantDbContext>().Database.Migrate();

               var context = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                context.Database.Migrate();
                EnsureSeedData(context);
            }

            Console.WriteLine("Done seeding database.");
            Console.WriteLine();
        }
        private static void EnsureSeedData(ConfigurationDbContext context)
        {
            if (!context.Clients.Any())
            {
                Console.WriteLine("Clients 正在初始化");
                foreach (var client in Config.GetClients)
                {
                    context.Clients.Add(client.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                Console.WriteLine("IdentityResources 正在初始化");
                foreach (var resource in Config.GetIdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                Console.WriteLine("ApiResources 正在初始化");
                foreach (var resource in Config.GetApiResources)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                Console.WriteLine("ApiScopes 正在初始化");
                foreach (var resource in Config.GetApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }
                context.SaveChanges();
            }
        }
    }
}
