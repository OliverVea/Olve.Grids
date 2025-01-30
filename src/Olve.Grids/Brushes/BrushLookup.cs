using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public class BrushLookup(IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)>? entries = null) : IBrushLookup
{
    private readonly Dictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush = entries != null
        ? entries.ToDictionary(x => (x.TileIndex, x.Corner), x => x.Brush)
        : new Dictionary<(TileIndex TileIndex, Corner Corner), BrushId>();

    public IEnumerable<BrushId> Brushes => _tileCornerToBrush.Values.Distinct();


    /// <inheritdoc />
    public OneOf<CornerBrushes, NotFound> GetBrushes(TileIndex tileIndex)
    {
        var cornerBrushes = new CornerBrushes();

        foreach (var corner in Corners.All)
        {
            var brush = GetBrushId(tileIndex, corner);

            if (brush.TryPickT0(out var actualBrushId, out _))
            {
                cornerBrushes[corner] = actualBrushId;
            }
        }

        return cornerBrushes;
    }

    /// <inheritdoc />
    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner) =>
        _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();

    /// <inheritdoc />
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

    /// <inheritdoc />
    public IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)> Entries =>
        _tileCornerToBrush.Select(x => (x.Key.Item1, x.Key.Item2, x.Value));

    /// <inheritdoc />
    public void SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes)
    {
        foreach (var corner in Corners.All)
        {
            SetCornerBrush(tileIndex, corner, cornerBrushes[corner]);
        }
    }


    /// <inheritdoc />
    public void SetCornerBrush(
        TileIndex tileIndex,
        Corner corner,
        BrushIdOrAny brushId
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

    /// <inheritdoc />
    public void Clear()
    {
        _tileCornerToBrush.Clear();
    }

    /// <inheritdoc />
    public void ClearTileBrushes(TileIndex tileIndex)
    {
        foreach (var corner in Corners.All)
        {
            _tileCornerToBrush.Remove((tileIndex, corner));
        }
    }

    /// <inheritdoc />
    public void ClearTileBrush(TileIndex tileIndex, Corner corner)
    {
        _tileCornerToBrush.Remove((tileIndex, corner));
    }
}