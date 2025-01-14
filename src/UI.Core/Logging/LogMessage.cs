using System.Diagnostics.CodeAnalysis;

namespace UI.Core.Logging;

public readonly record struct LogMessage
{
    private readonly object?[]? _args = null;

    public Exception? Exception { get; init; } = null;
    public LogLevel Level { get; init; } = LogLevel.Information;
    public string[]? Tags { get; init; } = null;
    public string? Source { get; init; } = null;
    public string Message { get; }

    public object?[] Args => _args ?? [ ];


    [SetsRequiredMembers]
    public LogMessage(string message)
    {
        Message = message;
    }

    [SetsRequiredMembers]
    public LogMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object[] args)
    {
        Message = message;
        _args = args;
    }
}