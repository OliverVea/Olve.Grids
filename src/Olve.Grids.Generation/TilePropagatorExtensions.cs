using DeBroglie;

namespace Olve.Grids.Generation;

public static class TilePropagatorExtensions
{
    public static IEnumerable<Position> Positions(this TilePropagator propagator)
    {
        for (var i = 0; i < propagator.Topology.IndexCount; i++)
        {
            propagator.Topology.GetCoord(i, out var x, out var y, out var z);
            
            var position = new Position(x, y);

            yield return position;
        }
    }
}