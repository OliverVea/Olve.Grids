using Olve.Grids.Brushes;
using Olve.Grids.Generation;

namespace Olve.Grids.IO.Readers;

/// <summary>
///     Reads a brush grid from a file.
/// </summary>
/// <param name="filePath">The file path.</param>
public class InputBrushFileReader(string filePath)
{
    /// <summary>
    ///     Loads a brush grid from the file.
    /// </summary>
    /// <param name="brushes">The brushes to use.</param>
    /// <returns>The loaded brush grid.</returns>
    public Result<BrushGrid> Load(IEnumerable<BrushId> brushes)
    {
        var lines = File.ReadAllLines(filePath);

        var sizeResult = GetSize(lines);
        if (sizeResult.TryPickProblems(out var problems, out var size))
        {
            return problems;
        }

        var grid = new BrushGrid(size);

        var brushLookup = brushes.ToDictionary(x => x.DisplayName[0], x => x);

        // Todo: refactor this please
        problems = new();
        var hadProblems = false;

        for (var y = 0; y < size.Height; y++)
        {
            for (var x = 0; x < size.Width; x++)
            {
                var c = lines[y][x];

                var brushResult = GetBrushId(brushLookup, c);

                if (brushResult.TryPickProblems(out var brushProblems, out var brushId))
                {
                    problems.Append(brushProblems);
                    hadProblems = true;
                    continue;
                }

                grid.SetBrush(new Position(x, y), brushId);
            }
        }

        if (hadProblems)
        {
            return problems;
        }

        return grid;
    }

    private Result<Size> GetSize(string[] lines)
    {
        var height = lines.Length;
        if (height == 0)
        {
            return new ResultProblem("File is empty.");
        }

        var width = lines[0].Length;
        if (lines.Any(x => x.Length != width))
        {
            return new ResultProblem("Inconsistent line lengths.");
        }

        return new Size(width, height);
    }

    private static Result<BrushIdOrAny> GetBrushId(Dictionary<char, BrushId> brushLookup, char c)
    {
        if (c == FileIOConstants.AnyBrushChar)
        {
            return BrushIdOrAny.Any;
        }

        if (!brushLookup.TryGetValue(c, out var brushId))
        {
            return new ResultProblem("Unknown brush character: '{0}'", c);
        }

        return new BrushIdOrAny(brushId);
    }
}