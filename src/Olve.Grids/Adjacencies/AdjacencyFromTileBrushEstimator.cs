using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public class AdjacencyFromTileBrushEstimator
{
    public void SetAdjacencies(
        IAdjacencyLookup adjacencyLookup,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        var tileIndices = new HashSet<TileIndex>();
        var brushIds = new HashSet<BrushId>();

        var lookup = new Dictionary<(TileIndex, Corner), BrushIdOrAny>();

        foreach (var (tileIndex, corner, brushId) in brushConfiguration)
        {
            tileIndices.Add(tileIndex);
            brushIds.Add(brushId);

            lookup[(tileIndex, corner)] = brushId;
        }

        var dict = new Dictionary<(Side, BrushId, BrushId), List<TileIndex>>();

        foreach (var tileIndex in tileIndices)
        {
            foreach (var side in Sides.All)
            {
                var (corner1, corner2) = Sides.GetCorners(side);

                var brushes1 = lookup
                    .GetValueOrDefault((tileIndex, corner1), new Any())
                    .Match(x => [ x, ], _ => brushIds);
                var brushes2 = lookup
                    .GetValueOrDefault((tileIndex, corner2), new Any())
                    .Match(x => [ x, ], _ => brushIds);

                foreach (var brush1 in brushes1)
                {
                    foreach (var brush2 in brushes2)
                    {
                        if (!dict.TryGetValue((side, brush1, brush2), out var list))
                        {
                            list = [ ];
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

            var otherTiles = dict.GetValueOrDefault((oppositeSide, brush1, brush2), [ ]);

            foreach (var tileFrom in tilesFrom)
            {
                foreach (var tileTo in otherTiles)
                {
                    adjacencyLookup.Add(tileFrom, tileTo, direction);
                }
            }
        }
    }
}