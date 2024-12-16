using DeBroglie;
using Olve.Grids.Grids;
using Olve.Utilities.Collections;
using OneOf.Types;

namespace Olve.Grids.DeBroglie;

public class TileSet(IEnumerable<TileIndex> tileIndices)
{
    private readonly BidirectionalDictionary<Tile, TileIndex> _tileIndexLookup = new(
        tileIndices.Select(x => new KeyValuePair<Tile, TileIndex>(new Tile(x), x)));

    public OneOf<TileIndex, NotFound> GetTileIndex(Tile tiles) => _tileIndexLookup.Get(tiles);
    public OneOf<Tile, NotFound> GetTile(TileIndex tileIndex) => _tileIndexLookup.Get(tileIndex);
}