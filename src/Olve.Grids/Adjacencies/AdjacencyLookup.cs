using Olve.Grids.Grids;

namespace Olve.Grids.Adjacencies;

public class AdjacencyLookup(IEnumerable<(TileIndex from, TileIndex to, AdjacencyDirection direction)>? values = null)
{
    private static readonly Dictionary<TileIndex, AdjacencyDirection> EmptyLookup = new();
    
    private Dictionary<TileIndex, Dictionary<TileIndex, AdjacencyDirection>> Lookup { get; } = 
        values?.GroupBy(pair => pair.from)
            .ToDictionary(
                group => group.Key,
                group => group.ToDictionary(pair => pair.to, pair => pair.direction)
            ) ?? new Dictionary<TileIndex, Dictionary<TileIndex, AdjacencyDirection>>();

    public AdjacencyDirection Get(TileIndex a, TileIndex b) => Lookup.GetValueOrDefault(a, EmptyLookup).GetValueOrDefault(b, AdjacencyDirection.None);

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
        Transform(a, b, x => x & ~direction);
    }
    
    private void Transform(TileIndex a, TileIndex b, Func<AdjacencyDirection, AdjacencyDirection> transform)
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

    public void Clear(TileIndex a, TileIndex b)
    {
        Lookup.GetValueOrDefault(a, EmptyLookup).Remove(b);
        Lookup.GetValueOrDefault(b, EmptyLookup).Remove(a);
    }
    
    public IEnumerable<TileIndex> GetNeighbors(TileIndex tileIndex)
    {
        return Lookup.GetValueOrDefault(tileIndex, EmptyLookup).Keys;
    }
    
    public IEnumerable<TileIndex> GetNeighborsInDirection(TileIndex tileIndex, AdjacencyDirection direction)
    {
        return Lookup.GetValueOrDefault(tileIndex, EmptyLookup)
            .Where(pair => pair.Value.HasFlag(direction))
            .Select(pair => pair.Key);
    }
}