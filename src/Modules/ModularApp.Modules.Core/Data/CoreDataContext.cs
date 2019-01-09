using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using ModularApp.Modules.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ModularApp.Modules.Core.Data
{
    public class CoreDataContext: DbContext
    {
        public CoreDataContext(DbContextOptions<CoreDataContext> options) : base(options) { }

        public DbSet<TestTable> Values { get; set; }
    }

}
