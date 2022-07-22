using Serilog;
using Serilog.Events;

namespace HermesCenter.Logger
{
    public enum Environment { Development, Production}

    public class LogManager : ILogManager
    {
        private readonly Serilog.ILogger _logger;

        public LogManager(Environment env, string instrumentationKey)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // ?
                .Enrich.FromLogContext()
                .WriteTo.Console();

            if(env == Environment.Development)
            {
                //
            }
            else
            {
                // TODO - it might be added to AppicationInsight Azure when it is in production
            }

            _logger = loggerConfiguration.CreateLogger();
        }

        public void Error(Exception ex, string message, string prefix = "")
        {

        }

        public void Error(string message, string prefix = "")
        {

        }

        public void Error(Exception ex, string template, params object[] protertyValues)
        {

        }

        public void Information(string message, string prefix = "")
        {

        }

        public void Information(string template, params object[] protertyValues)
        {

        }

        public void Warning(string template, params object[] protertyValues)
        {

        }
    }
}
