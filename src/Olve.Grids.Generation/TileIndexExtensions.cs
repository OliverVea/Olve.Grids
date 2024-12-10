using DeBroglie;
using Olve.Grids.Grids;

namespace Olve.Grids.Generation;

public static class TileIndexExtensions
{
    public static Tile ToTile(this TileIndex tileIndex)
    {
        return new Tile(tileIndex.Index);
    }
}