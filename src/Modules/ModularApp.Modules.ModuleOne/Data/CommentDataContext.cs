using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ModularApp.Modules.ModuleOne.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModularApp.Modules.ModuleOne.Data
{
    public class CommentDataContext : DbContext
    {
        public CommentDataContext(DbContextOptions<CommentDataContext> options) : base(options) { }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<CommentDataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlite(connectionString, o => o.MigrationsAssembly("ModularApp.WebHost"));
        }
    }

    public class CommentDataContextContextDbFactory : IDesignTimeDbContextFactory<CommentDataContext>
    {
        CommentDataContext IDesignTimeDbContextFactory<CommentDataContext>.CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<CommentDataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlite(connectionString, o => o.MigrationsAssembly("ModularApp.WebHost"));
            return new CommentDataContext(builder.Options);
        }
    }
}
