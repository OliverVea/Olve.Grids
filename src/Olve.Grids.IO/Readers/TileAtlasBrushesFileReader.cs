using System.Text;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;

namespace Olve.Grids.IO.Readers;

public class TileAtlasBrushesFileReader(string filePath)
{
    private const char NewLine = '\n';

    public OneOf<BrushLookupBuilder, FileParsingError> Load()
    {
        var text = File.ReadAllText(filePath);
        var sb = new StringBuilder(text);

        var whiteSpaceChars = text
            .Where(c => char.IsWhiteSpace(c) && c != NewLine)
            .Distinct()
            .ToArray();
        foreach (var whiteSpaceChar in whiteSpaceChars)
        {
            sb.Replace(whiteSpaceChar.ToString(), string.Empty);
        }

        var lines = sb
            .ToString()
            .Split(NewLine)
            .Where(x => x.Length != 0)
            .ToArray();

        var lineCount = lines.Length;

        if (lineCount % 2 != 0)
        {
            return FileParsingError.New("Invalid brush lookup file: line number must be even.");
        }

        var charCounts = lines
            .Select(x => x.Length)
            .ToArray();

        if (charCounts
                .Distinct()
                .Count()
            != 1)
        {
            return FileParsingError.New("Invalid brush lookup file: inconsistent line lengths.");
        }

        var charCount = charCounts.First();

        if (charCount % 2 != 0)
        {
            return FileParsingError.New("Invalid brush lookup file: line length must be even.");
        }

        var brushLookup = new Dictionary<char, BrushId>();

        var tileIndex = new TileIndex(0);
        var builder = new BrushLookupBuilder();

        for (var y = 0; y < lineCount; y += 2)
        {
            for (var x = 0; x < charCount; x += 2)
            {
                var upperLeft = GetBrushId(lines[y][x], brushLookup);
                var upperRight = GetBrushId(lines[y][x + 1], brushLookup);
                var lowerLeft = GetBrushId(lines[y + 1][x], brushLookup);
                var lowerRight = GetBrushId(lines[y + 1][x + 1], brushLookup);

                builder.SetCornerBrushes(
                    tileIndex,
                    new CornerBrushes(upperLeft, upperRight, lowerLeft, lowerRight)
                );

                tileIndex++;
            }
        }

        return builder;
    }

    private static OneOf<BrushId, Any> GetBrushId(char c, Dictionary<char, BrushId> brushLookup)
    {
        if (c == FileIOConstants.AnyBrushChar)
        {
            return new Any();
        }

        if (brushLookup.TryGetValue(c, out var existingBrushId))
        {
            return existingBrushId;
        }

        var newBrushId = BrushId.New(c.ToString());
        brushLookup.Add(c, newBrushId);

        return newBrushId;
    }
}