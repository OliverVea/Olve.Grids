using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public class AdjacencyLookup : IAdjacencyLookup
{
    private static readonly Dictionary<TileIndex, Direction> EmptyLookup = new();

    public AdjacencyLookup(IEnumerable<TileAdjacency>? values = null)
    {
        Lookup = new Dictionary<TileIndex, Dictionary<TileIndex, Direction>>();

        if (values is { })
        {
            foreach (var (from, to, direction) in values)
            {
                SetInternal((from, to, direction));
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

    public TileAdjacencies TileAdjacencies =>
        TileAdjacencies.FromEnumerable(Lookup
            .SelectMany(pair =>
                pair.Value.Select(innerPair => new TileAdjacency(pair.Key, innerPair.Key, innerPair.Value))
            )
            .Distinct());

    public void Set(TileAdjacency tileAdjacency)
    {
        SetInternal(tileAdjacency);
    }

    public void Add(TileAdjacency tileAdjacency)
    {
        Transform(tileAdjacency.From, tileAdjacency.To, x => x | tileAdjacency.Direction);
    }

    public void Remove(TileAdjacency tileAdjacency)
    {
        var (a, b, direction) = tileAdjacency;

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
            Remove((tile, neighbor, direction));
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

        SetInternal((a, b, result));
    }

    private void SetInternal(TileAdjacency tileAdjacency)
    {
        var (a, b, direction) = tileAdjacency;

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