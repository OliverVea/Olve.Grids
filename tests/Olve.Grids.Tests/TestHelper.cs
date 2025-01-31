using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

public class TestHelper
{
    public static IEnumerable<Func<Direction>> AllDirections()
    {
        return Directions.All.Select<Direction, Func<Direction>>(direction => () => direction);
    }

    public static IEnumerable<Func<Side>> AllSides()
    {
        return Sides.All.Select<Side, Func<Side>>(side => () => side);
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

    public static IEnumerable<TileIndex> GetTileIndices(int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(i => new TileIndex(i));
    }

    public static (TileIndex A, TileIndex B) GetTilePair()
    {
        var a = new TileIndex(0);
        var b = new TileIndex(1);

        return (a, b);
    }

    public static (TileIndex A, TileIndex B, TileIndex C) GetTileTriad()
    {
        var a = new TileIndex(0);
        var b = new TileIndex(1);
        var c = new TileIndex(2);

        return (a, b, c);
    }
}