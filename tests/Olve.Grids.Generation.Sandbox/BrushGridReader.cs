using Olve.Grids.Brushes;
using Olve.Grids.Generation.Generation;
using Olve.Grids.Generation.TileAtlas;
using Olve.Utilities.Types;

namespace Olve.Grids.Generation.Sandbox;

public static class BrushGridReader
{
    private const char Any = '?';
    
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
    
    private static OneOf.OneOf<BrushId, Any> GetBrushId(this Dictionary<char, BrushId> brushLookup, char c)
    {
        if (c == Any)
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