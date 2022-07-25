using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Exercise2.EF
{
    public class eShopDBContextFactory : IDesignTimeDbContextFactory<eShopDBContext>
    {
        public eShopDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json")
               .Build();
            var connectStrings = configuration.GetConnectionString("eShopConnectionStrings");
            var optionsBuilder = new DbContextOptionsBuilder<eShopDBContext>();
            optionsBuilder.UseMySQL(connectStrings);

            return new eShopDBContext(optionsBuilder.Options);
        }
    }
}
