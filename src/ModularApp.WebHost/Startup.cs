using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularApp.Modules.Core.Globals;

namespace ModularApp.WebHost
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IList<ModuleInfo> modules = new List<ModuleInfo>();


        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            _hostingEnvironment = env;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var root = _hostingEnvironment.ContentRootPath.Replace("\\ModularApp.WebHost", "");
            var moduleRootFolder = new DirectoryInfo(Path.Combine(root, "Modules"));
            var moduleFolders = moduleRootFolder.GetDirectories();
            foreach (var moduleFolder in moduleFolders)
            {
                var binFolder = new DirectoryInfo(Path.Combine(moduleFolder.FullName, "bin\\Debug"));
                if (!binFolder.Exists)
                {
                    continue;
                }

                foreach (var file in binFolder.GetFileSystemInfos("*.dll", SearchOption.AllDirectories))
                {
                    //Console.WriteLine(file.FullName);
                    Assembly assembly = null;
                    try
                    {
                        assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(file.FullName);
                    }
                    catch (FileLoadException ex)
                    {
                        assembly = Assembly.Load(new AssemblyName(Path.GetFileNameWithoutExtension(file.Name)));

                        if (assembly == null)
                        {
                            throw;
                        }

                        string loadedAssemblyVersion = FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
                        string tryToLoadAssemblyVersion = FileVersionInfo.GetVersionInfo(file.FullName).FileVersion;

                        // Or log the exception somewhere and don't add the module to list so that it will not be initialized
                        if (tryToLoadAssemblyVersion != loadedAssemblyVersion)
                        {
                            throw new Exception($"Cannot load {file.FullName} {tryToLoadAssemblyVersion} because {assembly.Location} {loadedAssemblyVersion} has been loaded");
                        }
                    }

                    if (assembly.FullName.Contains(moduleFolder.Name))
                    {
                        modules.Add(new ModuleInfo { Name = moduleFolder.Name, Assembly = assembly, Path = moduleFolder.FullName });
                        Console.WriteLine("=========================================");
                        Console.WriteLine("folder name: " + binFolder.FullName);
                        Console.WriteLine("assembly full name : " + assembly.FullName);
                        Console.WriteLine("=========================================");
                    }
                }
                Console.WriteLine("=========================================");
                Console.WriteLine(modules.Count);
                Console.WriteLine("=========================================");
            }

            foreach (var module in modules)
            {
                foreach (Type type in module.Assembly.GetTypes())
                {
                    if (type.IsSubclassOf(typeof(DbContext)))
                    {
                        //Console.WriteLine("Helloo: " + type.FullName);
                        //var secondListType = type.MakeGenericType(type);
                        //var secondList = Activator.CreateInstance(secondListType);
                        //services.AddDbContext<nameof(type.FullName)>(x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                        //var refType = dbType.;
                        //
                        //services.AddDbContext < nameof(type.FullName) > (x => x.UseSqlite(Configuration.GetConnectionString("DefaultConnection")));
                    }
                }
            }

            

            var mvcBuilder = services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            foreach (var module in modules)
            {
                // Register controller from modules
                mvcBuilder.AddApplicationPart(module.Assembly);
            }

            var moduleInitializerInterface = typeof(IModuleInitializer);
            foreach (var module in modules)
            {
                // Register dependency in modules
                var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModuleInitializer).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
                {
                    Console.WriteLine("moduleInitializerType : " + module.Name);
                    Console.WriteLine("moduleInitializerType : " + moduleInitializerType);
                    Console.WriteLine("moduleInitializerType : " + typeof(IModuleInitializer));
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.ConfigureServices(services, Configuration.GetConnectionString("DefaultConnection"));
                }
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            foreach (var module in modules)
            {
                // Register dependency in modules
                var moduleInitializerType = module.Assembly.GetTypes().Where(x => typeof(IModuleInitializer).IsAssignableFrom(x)).FirstOrDefault();
                if (moduleInitializerType != null && moduleInitializerType != typeof(IModuleInitializer))
                {
                    var moduleInitializer = (IModuleInitializer)Activator.CreateInstance(moduleInitializerType);
                    moduleInitializer.Configure(app, env);
                }
            }

            app.UseMvc();
        }
    }
}
