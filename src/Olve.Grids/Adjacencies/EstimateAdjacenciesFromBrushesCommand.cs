using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.CollectionExtensions;
using Olve.Utilities.Operations;

namespace Olve.Grids.Adjacencies;

public class EstimateAdjacenciesFromBrushesCommand : IOperation<EstimateAdjacenciesFromBrushesCommand.Request>
{
    public record Request(
        IAdjacencyLookup AdjacencyLookup,
        TileBrushes TileBrushes)
    {
        public IEnumerable<(TileIndex TileIndex, Direction Direction)> LockedAdjacencies { get; set; } = [ ];
    }

    public Result Execute(Request request)
    {
        var tileIndices = new HashSet<TileIndex>();
        var brushIds = new HashSet<BrushId>();

        var lookup = new Dictionary<(TileIndex, Corner), BrushIdOrAny>();

        foreach (var (tileIndex, corner, brushId) in request.TileBrushes)
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

        if (!request.LockedAdjacencies.TryAsReadOnlySet(out var lockedAdjacencies))
        {
            lockedAdjacencies = new HashSet<(TileIndex TileIndex, Direction Direction)>(request.LockedAdjacencies);
        }

        foreach (var ((side, brush1, brush2), tilesFrom) in dict)
        {
            var direction = side.ToAdjacencyDirection();
            var oppositeDirection = direction.Opposite();

            var oppositeSide = side.Opposite();

            var otherTiles = dict.GetValueOrDefault((oppositeSide, brush1, brush2), [ ]);

            foreach (var tileFrom in tilesFrom)
            {
                if (lockedAdjacencies.Contains((tileFrom, direction)))
                {
                    continue;
                }

                foreach (var tileTo in otherTiles)
                {
                    if (lockedAdjacencies.Contains((tileTo, oppositeDirection)))
                    {
                        continue;
                    }

                    TileAdjacency tileAdjacency = new(tileFrom, tileTo, direction);
                    request.AdjacencyLookup.Add(tileAdjacency);
                }
            }
        }

        return Result.Success();
    }
}