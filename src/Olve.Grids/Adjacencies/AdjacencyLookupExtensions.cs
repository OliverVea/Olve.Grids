using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public static class AdjacencyLookupExtensions
{
    public static bool Toggle(this IAdjacencyLookup adjacencyLookup, TileIndex from, TileIndex to, Direction direction)
    {
        var currentDirection = adjacencyLookup.Get(from, to);
        if (currentDirection.HasFlag(direction))
        {
            adjacencyLookup.Remove(from, to, currentDirection);
            return false;
        }

        adjacencyLookup.Set(from, to, currentDirection | direction);
        return true;
    }
}