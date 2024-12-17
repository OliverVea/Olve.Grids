namespace Olve.Grids.Primitives;

public static class AdjacencyDirectionExtensions
{
    public static Direction Opposite(this Direction direction)
    {
        var opposite = Direction.None;

        if (direction.HasFlag(Direction.Up))
        {
            opposite |= Direction.Down;
        }

        if (direction.HasFlag(Direction.Left))
        {
            opposite |= Direction.Right;
        }

        if (direction.HasFlag(Direction.Right))
        {
            opposite |= Direction.Left;
        }

        if (direction.HasFlag(Direction.Down))
        {
            opposite |= Direction.Up;
        }

        return opposite;
    }
}