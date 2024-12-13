using DeBroglie;
using Olve.Grids.Grids;

namespace Olve.Grids.DeBroglie;

public static class TileIndexExtensions
{
    public static Tile ToTile(this TileIndex tileIndex)
    {
        return new Tile(tileIndex.Index);
    }

    public static TileIndex ToTileIndex(this Tile? tile, TileIndex fallback = default)
    {
        if (tile?.Value == null)
        {
            return fallback;
        }
        
        var index = (int)tile.Value.Value;
        
        return new TileIndex(index);
    }
}