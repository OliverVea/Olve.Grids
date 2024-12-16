using System.Collections;
using Olve.Grids.Grids;

namespace Olve.Grids.Adjacencies;

public interface IAdjacencyLookup
{
    AdjacencyDirection Get(TileIndex a, TileIndex b);
    IEnumerable<TileIndex> GetNeighbors(TileIndex tileIndex);

    IEnumerable<TileIndex> GetNeighborsInDirection(
        TileIndex tileIndex,
        AdjacencyDirection direction
    );
}

public interface IAdjacencyLookupBuilder
{
    AdjacencyDirection Get(TileIndex a, TileIndex b);
    void Set(TileIndex a, TileIndex b, AdjacencyDirection direction);
    void Add(TileIndex a, TileIndex b, AdjacencyDirection direction);
    void Remove(TileIndex a, TileIndex b, AdjacencyDirection direction);
    void Clear(TileIndex a, TileIndex b);
    void Clear(TileIndex adjacencyTile, AdjacencyDirection direction);

    IAdjacencyLookup Build();
}

public class AdjacencyLookup(
    IEnumerable<(TileIndex from, TileIndex to, AdjacencyDirection direction)>? values = null
)
    : IAdjacencyLookup,
        IAdjacencyLookupBuilder,
        IEnumerable<(TileIndex from, TileIndex to, AdjacencyDirection direction)>
{
    private static readonly Dictionary<TileIndex, AdjacencyDirection> EmptyLookup = new();

    private Dictionary<TileIndex, Dictionary<TileIndex, AdjacencyDirection>> Lookup { get; } =
        values
            ?.GroupBy(pair => pair.from)
            .ToDictionary(
                group => group.Key,
                group => group.ToDictionary(pair => pair.to, pair => pair.direction)
            )
        ?? new Dictionary<TileIndex, Dictionary<TileIndex, AdjacencyDirection>>();

    public AdjacencyDirection Get(TileIndex a, TileIndex b)
    {
        return Lookup
            .GetValueOrDefault(a, EmptyLookup)
            .GetValueOrDefault(b, AdjacencyDirection.None);
    }

    public IEnumerable<TileIndex> GetNeighbors(TileIndex tileIndex)
    {
        return Lookup.GetValueOrDefault(tileIndex, EmptyLookup)
            .Keys;
    }

    public IEnumerable<TileIndex> GetNeighborsInDirection(
        TileIndex tileIndex,
        AdjacencyDirection direction
    )
    {
        return Lookup
            .GetValueOrDefault(tileIndex, EmptyLookup)
            .Where(pair => pair.Value.HasFlag(direction))
            .Select(pair => pair.Key);
    }

    public void Set(TileIndex a, TileIndex b, AdjacencyDirection direction)
    {
        SetInternal(a, b, direction);
    }

    public void Add(TileIndex a, TileIndex b, AdjacencyDirection direction)
    {
        Transform(a, b, x => x | direction);
    }

    public void Remove(TileIndex a, TileIndex b, AdjacencyDirection direction)
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

    public void Clear(TileIndex adjacencyTile, AdjacencyDirection direction)
    {
        var neighbors = GetNeighborsInDirection(adjacencyTile, direction)
            .ToList();

        foreach (var neighbor in neighbors)
        {
            Remove(adjacencyTile, neighbor, direction);
        }
    }

    public IAdjacencyLookup Build()
    {
        return new AdjacencyLookup(this);
    }

    public IEnumerator<(TileIndex from, TileIndex to, AdjacencyDirection direction)> GetEnumerator()
    {
        return Lookup
            .SelectMany(pair =>
                pair.Value.Select(innerPair => (pair.Key, innerPair.Key, innerPair.Value))
            )
            .Distinct()
            .GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void Transform(
        TileIndex a,
        TileIndex b,
        Func<AdjacencyDirection, AdjacencyDirection> transform
    )
    {
        var value = Get(a, b);

        var result = transform(value);

        SetInternal(a, b, result);
    }

    private void SetInternal(TileIndex a, TileIndex b, AdjacencyDirection direction)
    {
        if (direction == AdjacencyDirection.None)
        {
            Clear(a, b);
            return;
        }

        if (a == b)
        {
            if (!Lookup.ContainsKey(a))
            {
                Lookup[a] = new Dictionary<TileIndex, AdjacencyDirection>();
            }

            Lookup[a][b] = direction | direction.Opposite();

            return;
        }

        if (!Lookup.ContainsKey(a))
        {
            Lookup[a] = new Dictionary<TileIndex, AdjacencyDirection>();
        }

        Lookup[a][b] = direction;

        if (!Lookup.ContainsKey(b))
        {
            Lookup[b] = new Dictionary<TileIndex, AdjacencyDirection>();
        }

        Lookup[b][a] = direction.Opposite();
    }
}