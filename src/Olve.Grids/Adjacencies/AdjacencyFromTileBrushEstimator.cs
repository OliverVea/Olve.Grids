using Olve.Grids.Brushes;
using Olve.Grids.Grids;

namespace Olve.Grids.Adjacencies;

public enum Side
{
    Left,
    Right,
    Top,
    Bottom,
}

public static class Sides
{

    public static readonly IReadOnlyList<Side> All = [Side.Left, Side.Right, Side.Top, Side.Bottom];

    public static (Corner, Corner) GetCorners(Side side)
    {
        return side switch
        {
            Side.Left => (Corner.UpperLeft, Corner.LowerLeft),
            Side.Right => (Corner.UpperRight, Corner.LowerRight),
            Side.Top => (Corner.UpperLeft, Corner.UpperRight),
            Side.Bottom => (Corner.LowerLeft, Corner.LowerRight),
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }

    public static Side Opposite(this Side side)
    {
        return side switch
        {
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            Side.Top => Side.Bottom,
            Side.Bottom => Side.Top,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }

    public static AdjacencyDirection ToAdjacencyDirection(this Side side)
    {
        return side switch
        {
            Side.Left => AdjacencyDirection.Left,
            Side.Right => AdjacencyDirection.Right,
            Side.Top => AdjacencyDirection.Up,
            Side.Bottom => AdjacencyDirection.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }
}

public class AdjacencyFromTileBrushEstimator
{
    public void SetAdjacencies(
        IAdjacencyLookupBuilder adjacencyLookup,
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushConfiguration
    )
    {
        var tileIndices = new HashSet<TileIndex>();
        var brushIds = new HashSet<BrushId>();

        var lookup = new Dictionary<(TileIndex, Corner), OneOf<BrushId, Any>>();

        foreach (var (tileIndex, corner, brush) in brushConfiguration)
        {
            tileIndices.Add(tileIndex);

            if (brush.TryPickT0(out var brushId, out _))
            {
                brushIds.Add(brushId);
            }

            lookup[(tileIndex, corner)] = brush;
        }

        var dict = new Dictionary<(Side, BrushId, BrushId), List<TileIndex>>();

        foreach (var tileIndex in tileIndices)
        {
            foreach (var side in Sides.All)
            {
                var (corner1, corner2) = Sides.GetCorners(side);

                var brushes1 = lookup
                    .GetValueOrDefault((tileIndex, corner1), new Any())
                    .Match(x => [x], _ => brushIds);
                var brushes2 = lookup
                    .GetValueOrDefault((tileIndex, corner2), new Any())
                    .Match(x => [x], _ => brushIds);

                foreach (var brush1 in brushes1)
                {
                    foreach (var brush2 in brushes2)
                    {
                        if (!dict.TryGetValue((side, brush1, brush2), out var list))
                        {
                            list = [];
                            dict[(side, brush1, brush2)] = list;
                        }

                        list.Add(tileIndex);
                    }
                }
            }
        }

        foreach (var ((side, brush1, brush2), tilesFrom) in dict)
        {
            var direction = side.ToAdjacencyDirection();
            var oppositeSide = side.Opposite();

            var otherTiles = dict.GetValueOrDefault((oppositeSide, brush1, brush2), []);

            foreach (var tileFrom in tilesFrom)
            {
                foreach (var tileTo in otherTiles)
                {
                    adjacencyLookup.Add(tileFrom, tileTo, direction);
                }
            }
        }
    }

    public AdjacencyLookup GetAdjacencyLookup(
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> tiles
    )
    {
        var adjacencyLookup = new AdjacencyLookup();

        SetAdjacencies(adjacencyLookup, tiles);

        return adjacencyLookup;
    }
}