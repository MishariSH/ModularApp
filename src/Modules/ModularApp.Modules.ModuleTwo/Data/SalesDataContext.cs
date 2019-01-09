using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ModularApp.Modules.ModuleTwo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModularApp.Modules.ModuleTwo.Data
{
    public class SalesDataContext: DbContext
    {
        public SalesDataContext(DbContextOptions<SalesDataContext> options) : base(options) { }

        public DbSet<Sale> Sales { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
                var builder = new DbContextOptionsBuilder<SalesDataContext>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                builder.UseSqlite(connectionString, o => o.MigrationsAssembly("ModularApp.WebHost"));
            }
        }
    }

    public class SalesDataContextContextDbFactory : IDesignTimeDbContextFactory<SalesDataContext>
    {
        SalesDataContext IDesignTimeDbContextFactory<SalesDataContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<SalesDataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlite(connectionString, o => o.MigrationsAssembly("ModularApp.WebHost"));
            return new SalesDataContext(builder.Options);
        }
    }
}
