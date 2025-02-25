﻿namespace Olve.Grids.Primitives;

public static class DirectionExtensions
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

    public static bool IsCardinal(this Direction direction) => direction switch
    {
        Direction.Up => true,
        Direction.Left => true,
        Direction.Right => true,
        Direction.Down => true,
        _ => false,
    };

    public static Direction Combine(this IEnumerable<Direction> directions) => directions.Aggregate((a, b) => a | b);
}