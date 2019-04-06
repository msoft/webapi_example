using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
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
using log4net;
using log4net.Config;
using log4net.Repository;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using WebApiExample.Middlewares;

namespace WebApiExample
{
    /// <summary>
    /// Web API Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Creates a new instance of <see cref="Startup" />
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Represents a set of key/value application configuration properties.
        /// </summary>
        /// <returns></returns>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<FactoryActivatedMiddleware>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<ILoggerFactoryService, LoggerFactoryService>();
            services.AddTransient<IPizzaFlavourGeneratorService, PizzaFlavourGeneratorService>();
            services.AddSingleton<IPizzaFlavourRepositoryService, PizzaFlavourRepositoryService>();
            services.AddSingleton<IPizzaOrderRepositoryService, PizzaOrderRepositoryService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                // Permet de préciser de la documentation
                c.SwaggerDoc("v1", new Info { Title = "Pizza API", Version = "v1" });
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            var configFile = Path.Combine(env.ContentRootPath, "log4net.config");
            var repository = LogManager.GetRepository(Assembly.GetEntryAssembly());
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

            this.logger = LogManager.GetLogger(typeof(Startup));
            // app.Use((context, next) => {
            //     logger.Info("Invoking new middleware...");

            //     next.Invoke();

            //     logger.Info("Invoked.");
            //     return Task.CompletedTask;
            // });            

            //app.UseMiddleware<LoggingMiddleware>();
            // app.UseMiddleware<FactoryActivatedMiddleware>();
            //app.UseLoggingMiddleware();

            // app.Use 
            // app.Use(this.LoggingMiddlewareAsync1);
            // app.Use(this.LoggingMiddlewareAsync2);
            // //app.Use(this.StoppingMiddlewareAsync);
            // app.Use(this.LoggingMiddlewareAsync3);

            // app.Run
            // app.Use(this.LoggingMiddlewareAsync1);
            // app.Run(this.StoppingMiddlewareAsync);
            // app.Use(this.LoggingMiddlewareAsync3);

            // app.Map
            // app.Map("/api/PizzaFlavour", builder => {
            //     builder.UseMiddleware<LoggingMiddleware>();
            //     builder.Run(this.StoppingMiddlewareAsync);
            // });

            app.Use(this.WriteResponseBeforeNextMiddleware);

            // app.MapWhen
            app.MapWhen(
                context => context.Request.Path.StartsWithSegments("/health"),
                builder => {
                    builder.UseMiddleware<HealthCheckMiddleware>();
            });

            app.UseMvc();            
        }

        private ILog logger;

        private Task LoggingMiddleware(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Invoking new middleware...");

            next.Invoke();

            this.logger.Info("Invoked.");
            return Task.CompletedTask;
        }

        private async Task BadRequestMiddlewareAsync(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Invoking bad request middleware...");

            var emptyJsonString = "{ \"Message\": \"Your request is not properly formatted.\" }";

            // byte[] data = Encoding.UTF8.GetBytes(emptyJsonString);
            // context.Response.ContentType = "application/json";
            // await context.Response.Body.WriteAsync(data, 0, data.Length);
            
            context.Response.StatusCode = 400;
            context.Response.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json").ToString();
            await context.Response.WriteAsync(emptyJsonString, Encoding.UTF8);

            this.logger.Info("Bad request middleware invoked.");
        }

        private async Task StoppingMiddlewareAsync(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Invoking stopping middleware...");

            var emptyJsonString = "{}";
            context.Response.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json").ToString();
            await context.Response.WriteAsync(emptyJsonString, Encoding.UTF8);

            this.logger.Info("Stopping middleware invoked.");
        }

        private async Task StoppingMiddlewareAsync(HttpContext context)
        {
            this.logger.Info("Invoking stopping middleware...");

            var emptyJsonString = "{}";
            context.Response.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json").ToString();
            await context.Response.WriteAsync(emptyJsonString, Encoding.UTF8);

            this.logger.Info("Stopping middleware invoked.");
        }

        private async Task LoggingMiddlewareAsync1(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Executing 1st custom middleware...");

            await next.Invoke();

            this.logger.Info("1st custom middleware executed.");
        }

        private async Task LoggingMiddlewareAsync2(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Executing 2nd custom middleware...");

            await next.Invoke();

            this.logger.Info("2nd custom middleware executed.");
        }

        private async Task LoggingMiddlewareAsync3(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Executing 3rd custom middleware...");

            await next.Invoke();

            this.logger.Info("3rd custom middleware executed.");
        }

        private async Task WriteResponseBeforeNextMiddleware(HttpContext context, Func<Task> next)
        {
            this.logger.Info("Executing 3rd custom middleware...");

            byte[] data = Encoding.UTF8.GetBytes("{}");
            context.Response.ContentType = "application/json";
            await context.Response.Body.WriteAsync(data, 0, data.Length);
        
            this.logger.Info($"Response started: {context.Response.HasStarted}");
            await next.Invoke();                
            

            this.logger.Info("3rd custom middleware executed.");
        }
    }

    internal static class MiddlewareExtensions
    {
        public static void UseLoggingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<LoggingMiddleware>();
        }
    }
}

