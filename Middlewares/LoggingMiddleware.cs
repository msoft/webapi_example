using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using log4net;

namespace WebApiExample.Middlewares
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILog logger = LogManager.GetLogger(typeof(LoggingMiddleware));
        private readonly int instanceHashCode;

        public LoggingMiddleware(RequestDelegate next)
        {
            this.next = next;
            this.instanceHashCode = this.GetHashCode();
        }

        public async Task Invoke(HttpContext context)
        {
            this.logger.InfoFormat("Executing logging middleware (HashCode:{0})...",
                this.instanceHashCode);

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            await this.next(context);

            stopWatch.Stop();
            var executionTime = stopWatch.Elapsed;

            this.logger.InfoFormat("Logging middleware executed ({0} ms) (HashCode:{1}.", 
                executionTime.Milliseconds, this.instanceHashCode);
        }
    }
}