namespace Olve.Grids.Primitives;

public static class Sides
{
    public static readonly IReadOnlyList<Side> All = [ Side.Left, Side.Right, Side.Top, Side.Bottom, ];

    public static (Corner, Corner) GetCorners(Side side)
    {
        return side switch
        {
            Side.Left => (Corner.UpperLeft, Corner.LowerLeft),
            Side.Right => (Corner.UpperRight, Corner.LowerRight),
            Side.Top => (Corner.UpperLeft, Corner.UpperRight),
            Side.Bottom => (Corner.LowerLeft, Corner.LowerRight),
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }

    public static Side Opposite(this Side side)
    {
        return side switch
        {
            Side.Left => Side.Right,
            Side.Right => Side.Left,
            Side.Top => Side.Bottom,
            Side.Bottom => Side.Top,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }

    public static Direction ToAdjacencyDirection(this Side side)
    {
        return side switch
        {
            Side.Left => Direction.Left,
            Side.Right => Direction.Right,
            Side.Top => Direction.Up,
            Side.Bottom => Direction.Down,
            _ => throw new ArgumentOutOfRangeException(nameof(side), side, null),
        };
    }
}