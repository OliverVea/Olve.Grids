using DeBroglie.Topo;
using Olve.Grids.Adjacencies;

namespace Olve.Grids.DeBroglie;

public static class AdjacencyDirectionExtensions
{

    public static IEnumerable<Direction> GetDeBroglieDirections(this AdjacencyDirection adjacencyDirection)
    {
        if (adjacencyDirection.HasFlag(AdjacencyDirection.Up))
        {
            yield return Direction.YMinus;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Down))
        {
            yield return Direction.YPlus;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Left))
        {
            yield return Direction.XMinus;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Right))
        {
            yield return Direction.XPlus;
        }
    }
}