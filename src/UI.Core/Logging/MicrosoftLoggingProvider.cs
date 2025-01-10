using Microsoft.Extensions.Logging;

namespace UI.Core.Logging;

public class MicrosoftLoggingProvider(ILogger logger) : ILoggingProvider
{
    public void Log(LogMessage message)
    {
        var logLevel = ToMicrosoftLogLevel(message.Level);
        logger.Log(logLevel, message.Message, message.Args);
    }

    private Microsoft.Extensions.Logging.LogLevel ToMicrosoftLogLevel(LogLevel level)
    {
        return level.Value switch
        {
            LogLevel.FatalLevel => Microsoft.Extensions.Logging.LogLevel.Debug,
            LogLevel.ErrorLevel => Microsoft.Extensions.Logging.LogLevel.Error,
            LogLevel.WarningLevel => Microsoft.Extensions.Logging.LogLevel.Warning,
            LogLevel.InformationLevel => Microsoft.Extensions.Logging.LogLevel.Information,
            LogLevel.DebugLevel => Microsoft.Extensions.Logging.LogLevel.Debug,
            _ => Microsoft.Extensions.Logging.LogLevel.Information,
        };
    }
}

public class ConsoleLoggingProvider : ILoggingProvider
{
    public void Log(LogMessage message)
    {
#if DEBUG
        if (message.Exception is { } exception)
        {
            Console.WriteLine($"{message.Message}\n Got exception: {exception}", message.Args);
        }
        else
        {
            Console.WriteLine(message.Message, message.Args);
        }
#else
        Console.WriteLine(message.Message, message.Args);
#endif
    }
}