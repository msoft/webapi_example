using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Text;
using log4net;
using WebApiExample.Services;

namespace WebApiExample.Middlewares
{
    public class HealthCheckMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILog logger;
        private readonly int instanceHashCode;

        public HealthCheckMiddleware(RequestDelegate next, ILoggerFactoryService loggerFactory)
        {
            this.next = next;
            this.instanceHashCode = this.GetHashCode();
            this.logger = loggerFactory.GetNewLogger(typeof(HealthCheckMiddleware));
        }

        public async Task Invoke(HttpContext context)
        {
            this.logger.InfoFormat("Executing health check middleware (HashCode:{0})...",
                this.instanceHashCode);

            context.Response.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json").ToString();
            await context.Response.WriteAsync("{ \"health\": \"OK.\" }", 
                Encoding.UTF8);

            this.logger.InfoFormat("Health check middleware executed (HashCode:{0}.", 
                this.instanceHashCode);
        }
    }
}