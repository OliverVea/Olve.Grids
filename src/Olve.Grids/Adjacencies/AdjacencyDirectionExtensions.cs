namespace Olve.Grids.Adjacencies;

public static class AdjacencyDirectionExtensions
{
    public static AdjacencyDirection Opposite(this AdjacencyDirection adjacencyDirection)
    {
        var opposite = AdjacencyDirection.None;

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Up))
        {
            opposite |= AdjacencyDirection.Down;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Left))
        {
            opposite |= AdjacencyDirection.Right;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Right))
        {
            opposite |= AdjacencyDirection.Left;
        }

        if (adjacencyDirection.HasFlag(AdjacencyDirection.Down))
        {
            opposite |= AdjacencyDirection.Up;
        }

        return opposite;
    }
}