using Olve.Grids.Brushes;
using Olve.Grids.Generation;

namespace Olve.Grids.IO.Readers;

/// <summary>
/// Reads a brush grid from a file.
/// </summary>
/// <param name="filePath">The file path.</param>
public class InputBrushFileReader(string filePath)
{
    /// <summary>
    /// Loads a brush grid from the file.
    /// </summary>
    /// <param name="brushes">The brushes to use.</param>
    /// <returns>The loaded brush grid.</returns>
    public OneOf<BrushGrid, FileParsingError> Load(IEnumerable<BrushId> brushes)
    {
        var lines = File.ReadAllLines(filePath);

        if (!GetSize(lines)
                .TryPickT0(out var size, out var error))
        {
            return error;
        }

        var grid = new BrushGrid(size);

        var brushLookup = brushes.ToDictionary(x => x.DisplayName[0], x => x);

        List<FileParsingError> errors = [];

        for (var y = 0; y < size.Height; y++)
        {
            for (var x = 0; x < size.Width; x++)
            {
                var c = lines[y][x];

                var brushResult = GetBrushId(brushLookup, c);

                if (brushResult.TryPickT2(out var fileParsingError, out var brushId))
                {
                    errors.Add(fileParsingError);
                    continue;
                }

                grid.SetBrush(new Position(x, y), brushId);
            }
        }

        if (errors.Count > 0)
        {
            return FileParsingError.Combine(errors);
        }

        return grid;
    }

    private OneOf<Size, FileParsingError> GetSize(string[] lines)
    {
        var height = lines.Length;
        if (height == 0)
        {
            return FileParsingError.New("File is empty.");
        }

        var width = lines[0].Length;
        if (lines.Any(x => x.Length != width))
        {
            return FileParsingError.New("Inconsistent line lengths.");
        }

        return new Size(width, height);
    }

    private static OneOf<BrushId, Any, FileParsingError> GetBrushId(
        Dictionary<char, BrushId> brushLookup,
        char c
    )
    {
        if (c == FileIOConstants.AnyBrushChar)
        {
            return new Any();
        }

        if (!brushLookup.TryGetValue(c, out var brushId))
        {
            return FileParsingError.New("Unknown brush character: '{0}'.", c);
        }

        return brushId;
    }
}