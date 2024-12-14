using System.Collections;
using System.Collections.Frozen;
using Olve.Grids.Grids;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class BrushLookup : IBrushLookup
{
    private readonly FrozenDictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush;
    private readonly FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> _brushCornerToTiles;

    public FrozenSet<BrushId> AllBrushIds { get; }

    internal BrushLookup(
        FrozenSet<BrushId> allBrushIds,
        FrozenDictionary<(TileIndex, Corner), BrushId> tileCornerToBrush,
        FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> brushCornerToTiles
    )
    {
        AllBrushIds = allBrushIds;
        _tileCornerToBrush = tileCornerToBrush;
        _brushCornerToTiles = brushCornerToTiles;
    }

    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner)
    {
        return _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();
    }

    public OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner)
    {
        return _brushCornerToTiles.TryGetValue((brushId, corner), out var tiles)
            ? tiles
            : new NotFound();
    }

    public IEnumerable<BrushId> Brushes => AllBrushIds;

    public IEnumerator<(TileIndex, Corner, OneOf<BrushId, Any>)> GetEnumerator()
    {
        foreach (var (tileCorner, brushId) in _tileCornerToBrush)
        {
            yield return (tileCorner.Item1, tileCorner.Item2, brushId);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
