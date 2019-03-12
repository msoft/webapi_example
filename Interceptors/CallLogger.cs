using Autofac;
using Castle.DynamicProxy;
using System.Linq;
using System.IO;
using log4net;

namespace WebApiExample.Interceptors
{
    public class CallLogger : IInterceptor
    {
        private TextWriter _output;
        private ILog logger;

        public CallLogger(TextWriter output)
        {
            _output = output;
            this.logger = LogManager.GetLogger(typeof(CallLogger));
        }

        public void Intercept(IInvocation invocation)
        {
            this.logger.InfoFormat("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));
            _output.Write("Calling method {0} with parameters {1}... ",
                invocation.Method.Name,
                string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray()));

            invocation.Proceed();

            _output.WriteLine("Done: result was {0}.", invocation.ReturnValue);
            this.logger.InfoFormat("Done: result was {0}.", invocation.ReturnValue);
        }
    }
}