namespace Olve.Grids.Brushes;

public static class Corners
{
    public static Corner Opposite(this Corner corner)
    {
        return corner switch
        {
            Corner.UpperLeft => Corner.LowerRight,
            Corner.UpperRight => Corner.LowerLeft,
            Corner.LowerLeft => Corner.UpperRight,
            Corner.LowerRight => Corner.UpperLeft,
            _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null)
        };
    }

    public static readonly IReadOnlyList<Corner> All =
    [
        Corner.UpperLeft,
        Corner.UpperRight,
        Corner.LowerLeft,
        Corner.LowerRight
    ];
}