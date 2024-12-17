using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class BrushLookup(IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)>? entries = null) : IBrushLookup
{
    private readonly Dictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush = entries != null
        ? entries.ToDictionary(x => (x.TileIndex, x.Corner), x => x.Brush)
        : new Dictionary<(TileIndex TileIndex, Corner Corner), BrushId>();

    public IEnumerable<BrushId> Brushes => _tileCornerToBrush.Values.Distinct();

    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner) =>
        _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();

    public OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner)
    {
        var set = _tileCornerToBrush
            .Where(x => x.Value == brushId)
            .Select(x => x.Key.Item1)
            .ToHashSet();

        return set.Count > 0
            ? set
            : new NotFound();
    }

    public IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)> Entries =>
        _tileCornerToBrush.Select(x => (x.Key.Item1, x.Key.Item2, x.Value));

    public void SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes)
    {
        foreach (var corner in Corners.All)
        {
            SetCornerBrush(tileIndex, corner, cornerBrushes[corner]);
        }
    }

    public void SetCornerBrush(
        TileIndex tileIndex,
        Corner corner,
        OneOf<BrushId, Any> brushId
    )
    {
        if (brushId.TryPickT0(out var actualBrushId, out _))
        {
            _tileCornerToBrush[(tileIndex, corner)] = actualBrushId;
        }
        else
        {
            _tileCornerToBrush.Remove((tileIndex, corner));
        }
    }

    public void Clear()
    {
        _tileCornerToBrush.Clear();
    }

    public void ClearTileBrushes(TileIndex tileIndex)
    {
        foreach (var corner in Corners.All)
        {
            _tileCornerToBrush.Remove((tileIndex, corner));
        }
    }

    public void ClearTileBrush(TileIndex tileIndex, Corner corner)
    {
        _tileCornerToBrush.Remove((tileIndex, corner));
    }
}