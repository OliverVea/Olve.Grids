using System.Diagnostics.CodeAnalysis;

namespace UI.Core.Logging;

public readonly record struct LogMessage(
    Exception? Exception,
    LogLevel Level,
    [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
    string Message,
    string[]? Tags,
    string? Source,
    params object?[] Args);