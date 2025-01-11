

namespace Demo.Commands;

public readonly record struct VerbosityLevel(int Value)
{
    public static bool operator <(VerbosityLevel left, VerbosityLevel right) => left.Value < right.Value;
    public static bool operator >(VerbosityLevel left, VerbosityLevel right) => left.Value > right.Value;
    public static bool operator <=(VerbosityLevel left, VerbosityLevel right) => left.Value <= right.Value;
    public static bool operator >=(VerbosityLevel left, VerbosityLevel right) => left.Value >= right.Value;

    public static readonly VerbosityLevel Quiet = new(0);
    public bool IsQuiet => this >= Quiet;

    public static readonly VerbosityLevel Normal = new(1);
    public bool IsNormal => this >= Normal;

    public static readonly VerbosityLevel Verbose = new(2);
    public bool IsVerbose => this >= Verbose;
}

public static class VerbosityLevels
{

    public static Result<VerbosityLevel> Parse(string? value)
    {
        if (value is null)
        {
            return Result<VerbosityLevel>.Failure(new ResultProblem("The verbosity level must be specified."));
        }

        switch (value.ToLowerInvariant())
        {
            case "quiet":
            case "q":
                return VerbosityLevel.Quiet;
            case "normal":
            case "n":
                return VerbosityLevel.Normal;
            case "verbose":
            case "v":
                return VerbosityLevel.Verbose;
            default:
                return Result<VerbosityLevel>.Failure(new ResultProblem (
                    "The verbosity level must be 'Quiet', 'Normal', or 'Verbose' or 'q', 'n', or 'v'."));
        }
    }
}