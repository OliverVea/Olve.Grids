using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public class FrozenAdjacencyLookup(IEnumerable<(TileIndex from, TileIndex to, Direction direction)> values)
    : IReadOnlyAdjacencyLookup
{
    private static readonly Dictionary<TileIndex, Direction> Empty = new();

    private readonly Dictionary<TileIndex, Dictionary<TileIndex, Direction>> _adjacencies =
        values
            .GroupBy(pair => pair.from)
            .ToDictionary(
                group => group.Key,
                group => group.ToDictionary(pair => pair.to, pair => pair.direction)
            );

    public Direction Get(TileIndex a, TileIndex b) =>
        _adjacencies
            .GetValueOrDefault(a, Empty)
            .GetValueOrDefault(b, Direction.None);

    public IEnumerable<(TileIndex tileIndex, Direction direction)> GetNeighbors(TileIndex tileIndex) =>
        _adjacencies
            .GetValueOrDefault(tileIndex, Empty)
            .Select(pair => (pair.Key, pair.Value));

    public IEnumerable<TileIndex> GetNeighborsInDirection(TileIndex tileIndex, Direction direction)
        => _adjacencies
            .GetValueOrDefault(tileIndex, Empty)
            .Where(pair => pair.Value.HasFlag(direction))
            .Select(pair => pair.Key);


    public IEnumerable<(TileIndex from, TileIndex to, Direction direction)> Adjacencies =>
        _adjacencies
            .SelectMany(pair =>
                pair.Value.Select(innerPair => (pair.Key, innerPair.Key, innerPair.Value))
            )
            .Distinct();
}