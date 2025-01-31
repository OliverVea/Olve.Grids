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
        public IEnumerable<(TileIndex, Side)>? ToUpdate { get; set; }
        public HashSet<(TileIndex, Side)>? ToNotUpdate { get; set; }
    }

    public Result Execute(Request request)
    {
        var tileIndices = new HashSet<TileIndex>();

        var lookup = new Dictionary<(TileIndex, Corner), BrushId>();

        foreach (var (tileIndex, corner, brushId) in request.TileBrushes)
        {
            tileIndices.Add(tileIndex);

            lookup[(tileIndex, corner)] = brushId;
        }

        var tileSideToBrush = new Dictionary<(TileIndex, Side), (BrushId, BrushId)>();
        var brushSideToTiles = new Dictionary<(BrushId, BrushId, Side), HashSet<TileIndex>>();


        foreach (var tileIndex in tileIndices)
        {
            foreach (var side in Sides.All)
            {
                var (corner1, corner2) = Sides.GetCorners(side);

                if (!lookup.TryGetValue((tileIndex, corner1), out var brush1)
                    || !lookup.TryGetValue((tileIndex, corner2), out var brush2))
                {
                    continue;
                }

                tileSideToBrush[(tileIndex, side)] = (brush1, brush2);

                var tiles = brushSideToTiles.GetOrAdd((brush1, brush2, side), () => [ ]);
                tiles.Add(tileIndex);
            }
        }

        var toUpdate = request.ToUpdate ?? tileIndices.SelectMany(x => Sides.All.Select(y => (x, y)));

        var sidesByTile = toUpdate
            .GroupBy(x => x.Item1, x => x.Item2);

        foreach (var grouping in sidesByTile)
        {
            var from = grouping.Key;
            var sides = grouping.ToArray();

            foreach (var side in sides)
            {
                if (request.ToNotUpdate != null && request.ToNotUpdate.Contains((from, side)))
                {
                    continue;
                }

                foreach (var to in tileIndices)
                {
                    var oppositeSide = side.Opposite();

                    if (request.ToNotUpdate != null && request.ToNotUpdate.Contains((to, oppositeSide)))
                    {
                        continue;
                    }

                    var tileAdjacency = new TileAdjacency(from, to, side.ToDirection());
                    request.AdjacencyLookup.Remove(tileAdjacency);
                }
            }

            foreach (var side in sides)
            {
                if (request.ToNotUpdate != null && request.ToNotUpdate.Contains((from, side)))
                {
                    continue;
                }

                if (!tileSideToBrush.TryGetValue((from, side), out var fromBrushes))
                {
                    continue;
                }

                var oppositeSide = side.Opposite();

                var tos = brushSideToTiles.TryGetValue((fromBrushes.Item1, fromBrushes.Item2, oppositeSide),
                    out var toTiles)
                    ? toTiles
                    : [ ];

                foreach (var to in tos)
                {
                    if (request.ToNotUpdate != null && request.ToNotUpdate.Contains((to, oppositeSide)))
                    {
                        continue;
                    }

                    TileAdjacency tileAdjacency = new(from, to, side.ToDirection());
                    request.AdjacencyLookup.Add(tileAdjacency);
                }
            }
        }

        return Result.Success();
    }
}