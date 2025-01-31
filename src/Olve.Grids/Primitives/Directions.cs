using System.Diagnostics.CodeAnalysis;
using Olve.Utilities.Assertions;

namespace Olve.Grids.Primitives;

public static class Directions
{
    public static readonly IReadOnlyList<Direction>
        Cardinal = [ Direction.Up, Direction.Down, Direction.Left, Direction.Right, ];

    public static readonly IReadOnlyList<Direction> All = Enumerable
        .Range(1, (int)Direction.All)
        .Select(x => (Direction)x)
        .ToArray();

    [SuppressMessage("ReSharper", "SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault")]
    public static Side ToSide(this Direction direction)
    {
        Assert.That(() => direction.IsCardinal(), "Direction must be cardinal");

        return direction switch
        {
            Direction.Up => Side.Top,
            Direction.Down => Side.Bottom,
            Direction.Left => Side.Left,
            Direction.Right => Side.Right,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
    }
}