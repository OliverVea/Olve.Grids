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
    
    public AdjacencyDirection this[TileIndex a, TileIndex b]
    {
        get => Lookup.GetValueOrDefault(a, EmptyLookup).GetValueOrDefault(b, AdjacencyDirection.None);
        set
        {
            if (a == b)
            {
                if (!Lookup.ContainsKey(a))
                {
                    Lookup[a] = new Dictionary<TileIndex, AdjacencyDirection>();
                }

                Lookup[a][b] = value | value.Opposite();
                
                return;
            }
            
            if (!Lookup.ContainsKey(a))
            {
                Lookup[a] = new Dictionary<TileIndex, AdjacencyDirection>();
            }
            
            Lookup[a][b] = value;
            
            if (!Lookup.ContainsKey(b))
            {
                Lookup[b] = new Dictionary<TileIndex, AdjacencyDirection>();
            }
            
            Lookup[b][a] = value.Opposite();
        }
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