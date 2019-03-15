using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using WebApiExample.Services;
using Autofac;
using log4net;
using log4net.Config;
using log4net.Repository;
using System.Reflection;

namespace WebApiExample
{
    /// <summary>
    /// Web API Startup class
    /// </summary>
    public class Startup
    {
        private IHostingEnvironment hostingEnvironment;

        /// <summary>
        /// Creates a new instance of <see cref="Startup" />
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            this.hostingEnvironment = hostingEnvironment;

            var builder = new ConfigurationBuilder();
            this.Configuration = builder.Build();
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        /// <returns></returns>
        public IConfigurationRoot Configuration { get; }        

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // Permet de préciser de la documentation
                c.SwaggerDoc("v1", new Info { Title = "Pizza API", Version = "v1" });
            });
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var configFile = Path.Combine(env.ContentRootPath, "log4net.config");
            //ILoggerRepository repository = LogManager.CreateRepository("custom");
            var repository = log4net.LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(repository, new FileInfo(configFile));


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                // Ajoute un endpoint nommé d'une certaine façon
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza API V1");
            });

            app.UseMvc();
        }
    }
}
