using Olve.Grids.Brushes;
using Olve.Grids.Generation;

namespace Olve.Grids.FileIO;

public static class BrushGridReader
{
    public static BrushGrid ReadBrushGridFromFile(this TileAtlasBuilder builder, string fileName)
    {
        var lines = File.ReadAllLines(fileName);
        
        var size = new Size(lines.FirstOrDefault()?.Length ?? 0, lines.Length);
        
        var grid = new BrushGrid(size);

        var brushLookup = builder.BrushLookupBuilder.Brushes.
            ToDictionary(x => x.DisplayName[0], x => x);
        
        for (var y = 0; y < size.Height; y++)
        {
            for (var x = 0; x < size.Width; x++)
            {
                var c = lines[y][x];
                
                var brushId = brushLookup.GetBrushId(c);
                
                grid.SetBrush(new Position(x, y), brushId);
            }
        }
        
        return grid;
    }
    
    private static OneOf<BrushId, Any> GetBrushId(this Dictionary<char, BrushId> brushLookup, char c)
    {
        if (c == FileIOConstants.AnyBrushChar)
        {
            return new Any();
        }
        
        if (!brushLookup.TryGetValue(c, out var brushId))
        {
            throw new InvalidOperationException($"Unknown brush character '{c}'.");
        }
        
        return brushId;
    }
    
}