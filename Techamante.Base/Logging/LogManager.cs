using System;
using log4net;
using log4net.Config;

namespace Techamante.Base.Logging
{

    public class LogManager
    {
        private static LogManager _logManager;

        private readonly ILog _logger = log4net.LogManager.GetLogger(typeof(LogManager));

        private LogManager()
        {
            Configure();
        }

        private void Configure()
        {
            //string fileName = "log";
            //XmlConfigurator.ConfigureAndWatch(new FileInfo(fileName));
            XmlConfigurator.Configure();
        }


        private static void Initialize()
        {
            if (_logManager == null) _logManager = new LogManager();
        }

        public static void Info(object message)
        {
            Initialize();
            _logManager._logger.Info(message);
        }


        public static void Error(object message, Exception exception)
        {
            Initialize();
            _logManager._logger.Error(message, exception);
        }

    }


}
