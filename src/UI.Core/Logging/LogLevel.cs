namespace UI.Core.Logging;

public readonly record struct LogLevel(int Value)
{
    public const int DebugLevel = 0;
    public static LogLevel Debug => new(DebugLevel);

    public const int InformationLevel = 10;
    public static LogLevel Information => new(InformationLevel);

    public const int WarningLevel = 20;
    public static LogLevel Warning => new(WarningLevel);

    public const int ErrorLevel = 30;
    public static LogLevel Error => new(ErrorLevel);

    public const int FatalLevel = 40;
    public static LogLevel Fatal => new(FatalLevel);
}