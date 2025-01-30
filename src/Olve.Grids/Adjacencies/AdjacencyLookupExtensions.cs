namespace Olve.Grids.Adjacencies;

public static class AdjacencyLookupExtensions
{
    public static bool Toggle(this IAdjacencyLookup adjacencyLookup, TileAdjacency tileAdjacency)
    {
        var currentDirection = adjacencyLookup.Get(tileAdjacency.From, tileAdjacency.To);
        if (currentDirection.HasFlag(tileAdjacency.Direction))
        {
            adjacencyLookup.Remove(tileAdjacency);
            return false;
        }

        tileAdjacency = tileAdjacency with
        {
            Direction = tileAdjacency.Direction | currentDirection,
        };

        adjacencyLookup.Set(tileAdjacency);
        return true;
    }

    public static bool Set(this IAdjacencyLookup adjacencyLookup, TileAdjacency tileAdjacency, bool state)
    {
        var exists = adjacencyLookup
            .Get(tileAdjacency.From, tileAdjacency.To)
            .HasFlag(tileAdjacency.Direction);

        if (state)
        {
            adjacencyLookup.Set(tileAdjacency);

            return !exists;
        }

        adjacencyLookup.Remove(tileAdjacency);

        return exists;
    }
}