using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ModularApp.Modules.Core.Globals;
using ModularApp.Modules.ModuleTwo.Data;
using AutoMapper;

namespace ModularApp.Modules.ModuleTwo
{
    public class ModuleInitializer : IModuleInitializer
    {

        public void ConfigureServices(IServiceCollection services, string con)
        {
            services.AddDbContext<SalesDataContext>(x => x.UseSqlite(con));
            Mapper.Reset();
            services.AddAutoMapper();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

        }
    }
}
