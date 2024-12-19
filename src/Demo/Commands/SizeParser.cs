using OneOf.Types;

namespace Demo.Commands;

public static class SizeParser
{
    public static OneOf<Size, Error<string>> Parse(string? value)
    {
        if (value is null)
        {
            return new Error<string>("The size is required.");
        }

        var parts = value.Split('x');
        if (parts.Length != 2)
        {
            return new Error<string>("The size must be in the format '[width]x[height]', e.g. 4x4.");
        }

        if (!int.TryParse(parts[0], out var width))
        {
            return new Error<string>("The width must be a valid integer.");
        }

        if (!int.TryParse(parts[1], out var height))
        {
            return new Error<string>("The height must be a valid integer.");
        }

        return new Size(width, height);
    }
}