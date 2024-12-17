using DeBroglie.Topo;
using Olve.Grids.Adjacencies;
using Direction = Olve.Grids.Primitives.Direction;

namespace Olve.Grids.DeBroglie;

public static class AdjacencyDirectionExtensions
{

    public static IEnumerable<global::DeBroglie.Topo.Direction> GetDeBroglieDirections(this Direction direction)
    {
        if (direction.HasFlag(Direction.Up))
        {
            yield return global::DeBroglie.Topo.Direction.YMinus;
        }

        if (direction.HasFlag(Direction.Down))
        {
            yield return global::DeBroglie.Topo.Direction.YPlus;
        }

        if (direction.HasFlag(Direction.Left))
        {
            yield return global::DeBroglie.Topo.Direction.XMinus;
        }

        if (direction.HasFlag(Direction.Right))
        {
            yield return global::DeBroglie.Topo.Direction.XPlus;
        }
    }
}