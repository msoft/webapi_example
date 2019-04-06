using System;
using log4net;

namespace WebApiExample.Services
{
    public interface ILoggerFactoryService
    {
        ILog GetNewLogger(Type callerType);
    }

    internal class LoggerFactoryService : ILoggerFactoryService
    {
        public ILog GetNewLogger(Type callerType)
        {
            return LogManager.GetLogger(callerType);
        }
    }
}