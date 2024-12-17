using System.Collections.Frozen;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class FrozenBrushLookup : IReadOnlyBrushLookup
{
    private readonly FrozenSet<BrushId> _brushes;
    private readonly FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> _brushCornerToTiles;
    private readonly FrozenDictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush;

    public FrozenBrushLookup(IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)> items)
    {
        var brushCornerToTiles = new Dictionary<(BrushId, Corner), HashSet<TileIndex>>();
        var tileCornerToBrush = new Dictionary<(TileIndex, Corner), BrushId>();
        var brushes = new HashSet<BrushId>();

        foreach (var (tileIndex, corner, brushId) in items)
        {
            if (!brushCornerToTiles.TryGetValue((brushId, corner), out var tiles))
            {
                tiles = [ ];
                brushCornerToTiles[(brushId, corner)] = tiles;
            }

            tiles.Add(tileIndex);
            tileCornerToBrush[(tileIndex, corner)] = brushId;
            brushes.Add(brushId);
        }

        _brushes = brushes.ToFrozenSet();
        _brushCornerToTiles = brushCornerToTiles.ToFrozenDictionary(
            x => x.Key,
            x => x.Value.ToFrozenSet());
        _tileCornerToBrush = tileCornerToBrush.ToFrozenDictionary();
    }


    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner) =>
        _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();

    public OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner) =>
        _brushCornerToTiles.TryGetValue((brushId, corner), out var tiles)
            ? tiles
            : new NotFound();

    public IEnumerable<BrushId> Brushes => _brushes;
    public IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)> Entries => GetEntries();

    private IEnumerable<(TileIndex, Corner, BrushId)> GetEntries()
    {
        foreach (var (tileCorner, brushId) in _tileCornerToBrush)
        {
            yield return (tileCorner.Item1, tileCorner.Item2, brushId);
        }
    }
}