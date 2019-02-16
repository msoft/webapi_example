using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
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
using Newtonsoft.Json;

namespace WebApiExample
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
            .AddJsonOptions(options => 
                { 
                    options.SerializerSettings.Formatting = Formatting.Indented; 
                }); ;

            services.AddSingleton<IPizzaFlavourRepositoryService>(new PizzaFlavourRepositoryService());
            services.AddSingleton<IPizzaOrderRepositoryService, PizzaOrderRepositoryService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // Permet de préciser de la documentation
                c.SwaggerDoc("v1", new Info { 
                    Title = "Pizza API", 
                    Version = "v1",
                    Description = "API for pizza",
                    TermsOfService = "Terms of Service",
                    Contact = new Contact
                    {
                        Name = "Developer Name",
                        Email = "developer.name@example.com"
                    },
                    License = new License
                    {
                        Name = "Apache 2.0",
                        Url = "http://www.apache.org/licenses/LICENSE-2.0.html"
                    }
                
                });

                c.EnableAnnotations();

                var filePath = Path.Combine(System.AppContext.BaseDirectory, "WebApi.xml");
                c.IncludeXmlComments(filePath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
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
                c.DocumentTitle = "My Swagger UI";
                c.DisplayOperationId();
                //c.RoutePrefix = "pizza-api-docs";
            });

            app.UseMvc();
        }
    }
}
