using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using log4net;

namespace WebApiExample.Middlewares
{
    public class FactoryActivatedMiddleware : IMiddleware
    {
        private readonly ILog logger = LogManager.GetLogger(typeof(FactoryActivatedMiddleware));
        private readonly int instanceHashCode;

        public FactoryActivatedMiddleware()
        {
            this.instanceHashCode = this.GetHashCode();
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            this.logger.InfoFormat("Executing factory activated logging middleware (HashCode: {0})...",
                this.instanceHashCode);

            await next(context);

            this.logger.InfoFormat("Factory activated logging middleware executed (HashCode:{0}).",
                this.instanceHashCode);
        }
    }
}