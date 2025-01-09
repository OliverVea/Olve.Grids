namespace UI.Core;

public static class LogSeverities
{
    public const int Debug = 0;
    public const int Information = 1;
    public const int Warning = 2;
    public const int Error = 3;
    public const int Fatal = 4;

    public static Avalonia.Logging.LogEventLevel ToAvaloniaLogEventLevel(int severity)
    {
        return severity switch
        {
            Debug => Avalonia.Logging.LogEventLevel.Debug,
            Information => Avalonia.Logging.LogEventLevel.Information,
            Warning => Avalonia.Logging.LogEventLevel.Warning,
            Error => Avalonia.Logging.LogEventLevel.Error,
            Fatal => Avalonia.Logging.LogEventLevel.Fatal,
            _ => Avalonia.Logging.LogEventLevel.Information,
        };
    }
}