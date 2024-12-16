using System.Diagnostics.CodeAnalysis;

namespace Olve.Grids.IO;

public class FileParsingException(FileParsingError error)
    : Exception("An error occurred while parsing a file.")
{
    public FileParsingError Error { get; } = error;
}

/// <summary>
///     Represents a collection of errors that occurred while parsing a file.
/// </summary>
/// <param name="errors">The errors.</param>
public class FileParsingError(IReadOnlyList<FileParsingProblem> errors)
{

    /// <summary>
    ///     Gets all problems.
    /// </summary>
    public IReadOnlyList<FileParsingProblem> Problems { get; } = errors;

    internal static FileParsingError New(
        [StringSyntax(StringSyntaxAttribute.CompositeFormat)]
        string message,
        params object[]? args
    )
    {
        return new FileParsingError([ new FileParsingProblem(message, args), ]);
    }

    /// <summary>
    ///     Combines two errors.
    /// </summary>
    /// <param name="errors">The list of errors.</param>
    /// <returns></returns>
    public static FileParsingError Combine(params IEnumerable<FileParsingError> errors)
    {
        return new FileParsingError(errors
            .SelectMany(x => x.Problems)
            .ToList());
    }

    public Exception ToException()
    {
        return new FileParsingException(this);
    }
}

/// <summary>
///     Represents an error that occurred while parsing a file.
/// </summary>
/// <param name="Message">The error message.</param>
/// <param name="Args">The arguments for the message.</param>
public record FileParsingProblem(string Message, object[]? Args)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return string.Format(Message, Args ?? [ ]);
    }
}