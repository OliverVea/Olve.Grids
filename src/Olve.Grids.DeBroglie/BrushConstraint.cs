using DeBroglie;
using DeBroglie.Constraints;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;

namespace Olve.Grids.DeBroglie;

public class BrushConstraint(TileAtlas tileAtlas, BrushGrid brushGrid) : ITileConstraint
{
    private static readonly DeltaPosition UpperLeft = new(0, 0);
    private static readonly DeltaPosition UpperRight = new(1, 0);
    private static readonly DeltaPosition LowerLeft = new(0, 1);
    private static readonly DeltaPosition LowerRight = new(1, 1);
    
    public void Init(TilePropagator propagator)
    {
        foreach (var position in propagator.Positions())
        {
            var allowedTiles = GetAllowedTiles(position);
            if (allowedTiles == null)
            {
                continue;
            }

            var tiles = allowedTiles.Select(x => x.ToTile());

            propagator.Select(position.X, position.Y, 0, tiles);
        }
    }

    private HashSet<TileIndex>? GetAllowedTiles(Position position)
    {
        HashSet<TileIndex>? allowedTiles = null;
        
        allowedTiles = SetAllowedTiles(position, Corner.UpperLeft, UpperLeft, allowedTiles);
        allowedTiles = SetAllowedTiles(position, Corner.UpperRight, UpperRight, allowedTiles);
        allowedTiles = SetAllowedTiles(position, Corner.LowerLeft, LowerLeft, allowedTiles);
        allowedTiles = SetAllowedTiles(position, Corner.LowerRight, LowerRight, allowedTiles);
        
        return allowedTiles;
    }
    
    private HashSet<TileIndex>? SetAllowedTiles(Position position, Corner corner, DeltaPosition deltaPosition, HashSet<TileIndex>? allowedTiles)
    {
        var brush = brushGrid.GetBrush(position + deltaPosition);
        if (!brush.TryPickT0(out var brushId, out _))
        {
            return allowedTiles;
        }
        
        var tilesResult = tileAtlas.BrushLookup.GetTiles(brushId, corner.Opposite());
        if (!tilesResult.TryPickT0(out var tileIds, out _))
        {
            return allowedTiles;
        }
        
        if (allowedTiles == null)
        {
            return tileIds.ToHashSet();
        }

        allowedTiles.IntersectWith(tileIds);
        
        return allowedTiles;
    }

    public void Check(TilePropagator propagator)
    {
    }
}