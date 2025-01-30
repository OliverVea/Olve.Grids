using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public interface IAdjacencyLookup : IReadOnlyAdjacencyLookup
{
    void Set(TileAdjacency tileAdjacency);
    void Add(TileAdjacency tileAdjacency);
    void Remove(TileAdjacency tileAdjacency);
    void Clear(TileIndex a, TileIndex b);
    void Clear(TileIndex tile, Direction direction);
}