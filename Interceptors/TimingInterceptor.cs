using Autofac;
using Castle.DynamicProxy;
using System.Linq;
using System.Diagnostics;
using log4net;

namespace WebApiExample.Interceptors
{
    public class TimingInterceptor : IInterceptor
    {
        private ILog logger;

        public TimingInterceptor()
        {
            this.logger = LogManager.GetLogger(typeof(TimingInterceptor));
        }

        public void Intercept(IInvocation invocation)
        {
            this.logger.InfoFormat("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            invocation.Proceed();

            stopWatch.Start();
        
            this.logger.InfoFormat($"Function {invocation.Method.Name}: execution time {stopWatch.Elapsed}");
        }
    }
}