using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

public class TestHelper
{
    public static IEnumerable<Func<Direction>> AllDirections()
    {
        return Directions.All.Select<Direction, Func<Direction>>(direction => () => direction);
    }

    public static IEnumerable<Func<(Direction direction, Direction opposite)>> GetDirectionsWithOpposites()
    {
        return Directions.All.Select<Direction, Func<(Direction direction, Direction opposite)>>(
            direction => () => (direction, direction.Opposite()));
    }

    public static IEnumerable<Func<(Direction direction, Direction other)>> GetDirectionsWithOtherDirections()
    {
        foreach (var direction in Directions.All)
        {
            foreach (var otherDirection in Directions.All)
            {
                if (direction == otherDirection)
                {
                    continue;
                }

                yield return () => (direction, otherDirection);
            }
        }
    }

    public static (TileIndex from, TileIndex to) GetTilePair()
    {
        var from = new TileIndex(0);
        var to = new TileIndex(1);

        return (from, to);
    }
}