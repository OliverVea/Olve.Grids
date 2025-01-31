using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.CollectionExtensions;
using Olve.Utilities.Operations;

namespace Olve.Grids.Adjacencies;

public class EstimateAdjacenciesFromBrushesOperation : IOperation<EstimateAdjacenciesFromBrushesOperation.Request>
{
    public record Request(
        IAdjacencyLookup AdjacencyLookup,
        TileBrushes TileBrushes)
    {
        public IEnumerable<(TileIndex, Side)>? ToUpdate { get; set; }
        public IReadOnlySet<(TileIndex, Side)>? ToNotUpdate { get; set; }
    }

    public Result Execute(Request request)
    {
        var tileIndices = ExtractTileIndices(request);
        var lookup = CreateLookupDictionary(request);
        var (tileSideToBrush, brushSideToTiles) = BuildBrushMappings(tileIndices, lookup);
        var toUpdate = request.ToUpdate ?? tileIndices.SelectMany(x => Sides.All.Select(y => (x, y)));
        var sidesByTile = toUpdate.GroupBy(x => x.Item1, x => x.Item2);

        UpdateAdjacencyLookup(request, sidesByTile, tileIndices, tileSideToBrush, brushSideToTiles);

        return Result.Success();
    }

    private HashSet<TileIndex> ExtractTileIndices(Request request)
    {
        return request
            .TileBrushes.Select(tb => tb.TileIndex)
            .ToHashSet();
    }

    private Dictionary<(TileIndex, Corner), BrushId> CreateLookupDictionary(Request request)
    {
        return request.TileBrushes.ToDictionary(
            tb => (tb.TileIndex, tb.Corner),
            tb => tb.BrushId
        );
    }

    private (Dictionary<(TileIndex, Side), (BrushId, BrushId)>, Dictionary<(BrushId, BrushId, Side), HashSet<TileIndex>>)
        BuildBrushMappings(HashSet<TileIndex> tileIndices, Dictionary<(TileIndex, Corner), BrushId> lookup)
    {
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

                var tiles = brushSideToTiles.GetOrAdd((brush1, brush2, side), () => new HashSet<TileIndex>());
                tiles.Add(tileIndex);
            }
        }

        return (tileSideToBrush, brushSideToTiles);
    }

    private void UpdateAdjacencyLookup(
        Request request,
        IEnumerable<IGrouping<TileIndex, Side>> sidesByTile,
        HashSet<TileIndex> tileIndices,
        Dictionary<(TileIndex, Side), (BrushId, BrushId)> tileSideToBrush,
        Dictionary<(BrushId, BrushId, Side), HashSet<TileIndex>> brushSideToTiles)
    {
        foreach (var grouping in sidesByTile)
        {
            var from = grouping.Key;
            var sides = grouping.ToArray();

            RemoveObsoleteAdjacencies(request, from, sides, tileIndices);
            AddNewAdjacencies(request, from, sides, tileSideToBrush, brushSideToTiles);
        }
    }

    private void RemoveObsoleteAdjacencies(Request request, TileIndex from, Side[] sides, HashSet<TileIndex> tileIndices)
    {
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
    }

    private void AddNewAdjacencies(
        Request request,
        TileIndex from,
        Side[] sides,
        Dictionary<(TileIndex, Side), (BrushId, BrushId)> tileSideToBrush,
        Dictionary<(BrushId, BrushId, Side), HashSet<TileIndex>> brushSideToTiles)
    {
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
            var tos = brushSideToTiles.TryGetValue((fromBrushes.Item1, fromBrushes.Item2, oppositeSide), out var toTiles)
                ? toTiles
                : new HashSet<TileIndex>();

            foreach (var to in tos)
            {
                if (request.ToNotUpdate != null && request.ToNotUpdate.Contains((to, oppositeSide)))
                {
                    continue;
                }

                var tileAdjacency = new TileAdjacency(from, to, side.ToDirection());
                request.AdjacencyLookup.Add(tileAdjacency);
            }
        }
    }
}