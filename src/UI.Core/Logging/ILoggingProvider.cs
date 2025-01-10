namespace UI.Core.Logging;

public interface ILoggingProvider
{
    void Log(LogMessage message);
}