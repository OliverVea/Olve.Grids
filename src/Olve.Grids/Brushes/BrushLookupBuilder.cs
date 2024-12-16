using System.Collections;
using System.Collections.Frozen;
using Olve.Grids.Grids;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class BrushLookupBuilder : IBrushLookupBuilder
{
    private readonly Dictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush = new();

    public IEnumerable<BrushId> Brushes => _tileCornerToBrush.Values.Distinct();

    public OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner)
    {
        return _tileCornerToBrush.TryGetValue((tileIndex, corner), out var brushId)
            ? brushId
            : new NotFound();
    }

    public IBrushLookupBuilder SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes)
    {
        foreach (var corner in Corners.All)
        {
            SetCornerBrush(tileIndex, corner, cornerBrushes[corner]);
        }

        return this;
    }

    public IBrushLookupBuilder SetCornerBrush(
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

        return this;
    }

    public IBrushLookupBuilder Clear()
    {
        _tileCornerToBrush.Clear();

        return this;
    }

    public IBrushLookupBuilder ClearTileBrushes(TileIndex tileIndex)
    {
        foreach (var corner in Corners.All)
        {
            _tileCornerToBrush.Remove((tileIndex, corner));
        }

        return this;
    }

    public IBrushLookupBuilder ClearTileBrush(TileIndex tileIndex, Corner corner)
    {
        _tileCornerToBrush.Remove((tileIndex, corner));

        return this;
    }

    public IBrushLookup Build()
    {
        var allBrushIds = GetAllBrushIds();
        var tileCornerToBrush = _tileCornerToBrush.ToFrozenDictionary();
        var brushCornerToTiles = GetBrushCornerToTiles(allBrushIds, tileCornerToBrush);

        return new BrushLookup(allBrushIds, tileCornerToBrush, brushCornerToTiles);
    }

    private FrozenSet<BrushId> GetAllBrushIds()
    {
        return _tileCornerToBrush.Values.ToFrozenSet();
    }

    private FrozenDictionary<(BrushId, Corner), FrozenSet<TileIndex>> GetBrushCornerToTiles(
        FrozenSet<BrushId> allBrushes,
        FrozenDictionary<(TileIndex, Corner), BrushId> tileCornerToBrush
    )
    {
        var brushCornerToTiles = new Dictionary<(BrushId, Corner), HashSet<TileIndex>>();

        foreach (var brushId in allBrushes)
        {
            foreach (var corner in Corners.All)
            {
                brushCornerToTiles[(brushId, corner)] = [];
            }
        }

        foreach (var ((tileIndex, corner), brushId) in tileCornerToBrush)
        {
            var oppositeCorner = corner.Opposite();
            brushCornerToTiles[(brushId, oppositeCorner)].Add(tileIndex);
        }

        return brushCornerToTiles
            .Where(x => x.Value.Count > 0)
            .ToFrozenDictionary(x => x.Key, x => x.Value.ToFrozenSet());
    }

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