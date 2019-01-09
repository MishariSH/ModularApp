using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModularApp.Modules.Core.Globals
{
    public interface IModuleInitializer
    {
        void ConfigureServices(IServiceCollection serviceCollection, string con);

        void Configure(IApplicationBuilder app, IHostingEnvironment env);
    }
}
