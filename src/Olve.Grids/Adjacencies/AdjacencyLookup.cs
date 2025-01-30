using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public class AdjacencyLookup : IAdjacencyLookup
{
    private static readonly Dictionary<TileIndex, Direction> EmptyLookup = new();

    public AdjacencyLookup(IEnumerable<(TileIndex from, TileIndex to, Direction direction)>? values = null)
    {
        Lookup = new Dictionary<TileIndex, Dictionary<TileIndex, Direction>>();

        if (values is { })
        {
            foreach (var (from, to, direction) in values)
            {
                SetInternal(from, to, direction);
            }
        }
    }

    private Dictionary<TileIndex, Dictionary<TileIndex, Direction>> Lookup { get; }

    public Direction Get(TileIndex a, TileIndex b) =>
        Lookup
            .GetValueOrDefault(a, EmptyLookup)
            .GetValueOrDefault(b, Direction.None);

    public IEnumerable<(TileIndex tileIndex, Direction direction)> GetNeighbors(TileIndex tileIndex) =>
        Lookup
            .GetValueOrDefault(tileIndex, EmptyLookup)
            .Select(pair => (pair.Key, pair.Value));

    public IEnumerable<TileIndex> GetNeighborsInDirection(
        TileIndex tileIndex,
        Direction direction
    )
    {
        return Lookup
            .GetValueOrDefault(tileIndex, EmptyLookup)
            .Where(pair => pair.Value.HasFlag(direction))
            .Select(pair => pair.Key);
    }

    public IEnumerable<(TileIndex from, TileIndex to, Direction direction)> Adjacencies =>
        Lookup
            .SelectMany(pair =>
                pair.Value.Select(innerPair => (pair.Key, innerPair.Key, innerPair.Value))
            )
            .Distinct();

    public void Set(TileIndex a, TileIndex b, Direction direction)
    {
        SetInternal(a, b, direction);
    }

    public void Add(TileIndex a, TileIndex b, Direction direction)
    {
        Transform(a, b, x => x | direction);
    }

    public void Remove(TileIndex a, TileIndex b, Direction direction)
    {
        Transform(a, b, x => a != b ? x & ~direction : x & ~direction & ~direction.Opposite());
    }

    public void Clear(TileIndex a, TileIndex b)
    {
        Lookup
            .GetValueOrDefault(a, EmptyLookup)
            .Remove(b);
        Lookup
            .GetValueOrDefault(b, EmptyLookup)
            .Remove(a);
    }

    public void Clear(TileIndex tile, Direction direction)
    {
        var neighbors = GetNeighborsInDirection(tile, direction)
            .ToList();

        foreach (var neighbor in neighbors)
        {
            Remove(tile, neighbor, direction);
        }
    }

    private void Transform(
        TileIndex a,
        TileIndex b,
        Func<Direction, Direction> transform
    )
    {
        var value = Get(a, b);

        var result = transform(value);

        SetInternal(a, b, result);
    }

    private void SetInternal(TileIndex a, TileIndex b, Direction direction)
    {
        if (direction == Direction.None)
        {
            Clear(a, b);
            return;
        }

        if (a == b)
        {
            if (!Lookup.ContainsKey(a))
            {
                Lookup[a] = new Dictionary<TileIndex, Direction>();
            }

            Lookup[a][b] = direction | direction.Opposite();

            return;
        }

        if (!Lookup.ContainsKey(a))
        {
            Lookup[a] = new Dictionary<TileIndex, Direction>();
        }

        Lookup[a][b] = direction;

        if (!Lookup.ContainsKey(b))
        {
            Lookup[b] = new Dictionary<TileIndex, Direction>();
        }

        Lookup[b][a] = direction.Opposite();
    }
}