using System.Collections.Frozen;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class FrozenBrushLookup : IReadOnlyBrushLookup
{
    private readonly FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> _brushCornerToTiles;
    private readonly FrozenDictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush;

    internal FrozenBrushLookup(
        FrozenSet<BrushId> allBrushIds,
        FrozenDictionary<(TileIndex, Corner), BrushId> tileCornerToBrush,
        FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> brushCornerToTiles
    )
    {
        AllBrushIds = allBrushIds;
        _tileCornerToBrush = tileCornerToBrush;
        _brushCornerToTiles = brushCornerToTiles;
    }

    public FrozenSet<BrushId> AllBrushIds { get; }

    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner) =>
        _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();

    public OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner) =>
        _brushCornerToTiles.TryGetValue((brushId, corner), out var tiles)
            ? tiles
            : new NotFound();

    public IEnumerable<BrushId> Brushes => AllBrushIds;
    public IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> Entries => GetEntries();

    private IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> GetEntries()
    {
        foreach (var (tileCorner, brushId) in _tileCornerToBrush)
        {
            yield return (tileCorner.Item1, tileCorner.Item2, brushId);
        }
    }
}