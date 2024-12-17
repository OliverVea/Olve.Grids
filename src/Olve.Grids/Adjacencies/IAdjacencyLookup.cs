using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public interface IAdjacencyLookup : IReadOnlyAdjacencyLookup
{
    void Set(TileIndex a, TileIndex b, Direction direction);
    void Add(TileIndex a, TileIndex b, Direction direction);
    void Remove(TileIndex a, TileIndex b, Direction direction);
    void Clear(TileIndex a, TileIndex b);
    void Clear(TileIndex tile, Direction direction);
}