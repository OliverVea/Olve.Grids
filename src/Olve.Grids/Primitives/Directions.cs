namespace Olve.Grids.Primitives;

public static class Directions
{
    public static readonly IReadOnlyList<Direction>
        Cardinal = [ Direction.Up, Direction.Down, Direction.Left, Direction.Right, ];

    public static readonly IReadOnlyList<Direction> All = Enumerable
        .Range(1, (int)Direction.All)
        .Select(x => (Direction)x)
        .ToArray();
}