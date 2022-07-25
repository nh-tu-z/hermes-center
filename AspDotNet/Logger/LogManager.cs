using Serilog;
using Serilog.Events;

namespace HermesCenter.Logger
{
    public enum Environment { Development, Production}

    public class LogManager : ILogManager
    {
        private readonly Serilog.ILogger _logger;

        public LogManager(Environment env, string instrumentationKey = "" /* implement with ApplicationInsight */)
        {
            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // ?
                .Enrich.FromLogContext()
                .WriteTo.Console();

            if(env == Environment.Development)
            {
                loggerConfiguration.WriteTo.Debug();
            }
            else
            {
                // TODO - it might be added to AppicationInsight Azure (another sink) when it is in production
            }

            _logger = loggerConfiguration.CreateLogger();
        }

        public void Error(Exception ex, string message, string prefix = "")
        {
            string msg = (prefix != string.Empty) ? $"[{prefix}] {message}" : message;
            _logger.Error(ex, msg);
        }

        public void Error(string message, string prefix = "")
        {
            string msg = (prefix != string.Empty) ? $"[{prefix}] {message}" : message;
            _logger.Error(msg);
        }

        public void Error(Exception ex, string template, params object[] protertyValues)
        {

        }

        public void Information(string message, string prefix = "")
        {
            string msg = (prefix != string.Empty) ? $"[{prefix}] {message}" : message;
            _logger.Information(msg);
        }

        public void Information(string template, params object[] protertyValues)
        {
            _logger.Information(template, protertyValues);
        }

        public void Warning(string template, params object[] protertyValues)
        {

        }
    }
}
