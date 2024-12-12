using System.Collections.Frozen;
using Olve.Grids.Grids;

namespace Olve.Grids.Brushes;

public class BrushLookupBuilder
{
    private readonly Dictionary<(TileIndex, Corner), BrushId> _tileCornerToBrush = new();
    
    public IEnumerable<BrushId> Brushes => _tileCornerToBrush.Values.Distinct(); 
    
    public BrushLookupBuilder SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes)
    {
        foreach (var corner in Corners.All)
        {
            SetCornerBrush(tileIndex, corner, cornerBrushes[corner]);
        }
        
        return this;
    }
    
    public BrushLookupBuilder SetCornerBrush(TileIndex tileIndex, Corner corner, OneOf<BrushId, Any> brushId)
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
    
    public BrushLookupBuilder Clear()
    {
        _tileCornerToBrush.Clear();
        
        return this;
    }
    
    public BrushLookupBuilder ClearTileBrushes(TileIndex tileIndex)
    {
        foreach (var corner in Corners.All)
        {
            _tileCornerToBrush.Remove((tileIndex, corner));
        }

        return this;
    }
    
    public BrushLookupBuilder ClearTileBrush(TileIndex tileIndex, Corner corner)
    {
        _tileCornerToBrush.Remove((tileIndex, corner));
        
        return this;
    }
    
    public BrushLookup Build()
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
        FrozenDictionary<(TileIndex, Corner), BrushId> tileCornerToBrush)
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
            .ToFrozenDictionary(
                x => x.Key,
                x => x.Value.ToFrozenSet());
    }
}