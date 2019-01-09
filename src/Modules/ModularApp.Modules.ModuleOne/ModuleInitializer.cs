using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularApp.Modules.Core.Globals;
using ModularApp.Modules.ModuleOne.Data;
using System;

namespace ModularApp.Modules.ModuleOne
{
    public class ModuleInitializer : IModuleInitializer
    {

        public void ConfigureServices(IServiceCollection services, string con)
        {
            Console.WriteLine("HEHEHEHEHEHEHEHEHEHEHE");
            services.AddDbContext<CommentDataContext>(x => x.UseSqlite(con));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }
}
