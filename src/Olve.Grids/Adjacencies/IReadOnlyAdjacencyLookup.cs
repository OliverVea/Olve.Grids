using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public interface IReadOnlyAdjacencyLookup
{
    Direction Get(TileIndex a, TileIndex b);
    IEnumerable<TileIndex> GetNeighbors(TileIndex tileIndex);

    IEnumerable<TileIndex> GetNeighborsInDirection(
        TileIndex tileIndex,
        Direction direction
    );

    IEnumerable<(TileIndex from, TileIndex to, Direction direction)> Adjacencies { get; }
}