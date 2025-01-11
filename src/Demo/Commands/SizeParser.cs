

namespace Demo.Commands;

public static class SizeParser
{
    public static Result<Size> Parse(string? value)
    {
        if (value is null)
        {
            return Result<Size>.Failure(new ResultProblem("The size must be specified."));
        }

        var parts = value.Split('x');
        if (parts.Length != 2)
        {
            return Result<Size>.Failure(new ResultProblem("The size '{0}' is not in the format 'widthxheight'.", value));
        }

        if (!int.TryParse(parts[0], out var width))
        {
            return Result<Size>.Failure(new ResultProblem("The width '{0}' is not a valid integer.", parts[0]));
        }

        if (!int.TryParse(parts[1], out var height))
        {
            return Result<Size>.Failure(new ResultProblem("The height '{0}' is not a valid integer.", parts[1]));
        }

        return new Size(width, height);
    }
}