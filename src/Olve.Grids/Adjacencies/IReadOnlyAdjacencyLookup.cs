using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public interface IReadOnlyAdjacencyLookup
{
    TileAdjacencies TileAdjacencies { get; }
    Direction Get(TileIndex a, TileIndex b);
    IEnumerable<(TileIndex tileIndex, Direction direction)> GetNeighbors(TileIndex tileIndex);

    IEnumerable<TileIndex> GetNeighborsInDirection(
        TileIndex tileIndex,
        Direction direction
    );
}