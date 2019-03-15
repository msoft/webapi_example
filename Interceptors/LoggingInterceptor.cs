using Autofac;
using Castle.DynamicProxy;
using System.Linq;
using System.IO;
using log4net;

namespace WebApiExample.Interceptors
{
    public class LoggingInterceptor : IInterceptor
    {
        private ILog logger;

        public LoggingInterceptor()
        {
            this.logger = LogManager.GetLogger(typeof(LoggingInterceptor));
        }

        public void Intercept(IInvocation invocation)
        {
            this.logger.InfoFormat("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            this.logger.InfoFormat("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}