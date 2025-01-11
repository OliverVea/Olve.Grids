using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public record CornerBrushes
{
    public BrushIdOrAny UpperLeft { get; set; } = new Any();
    public BrushIdOrAny UpperRight { get; set; } = new Any();
    public BrushIdOrAny LowerLeft { get; set; } = new Any();
    public BrushIdOrAny LowerRight { get; set; } = new Any();

    public BrushIdOrAny this[Corner corner]
    {
        get
        {
            return corner switch
            {
                Corner.UpperLeft => UpperLeft,
                Corner.UpperRight => UpperRight,
                Corner.LowerLeft => LowerLeft,
                Corner.LowerRight => LowerRight,
                _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null),
            };
        }
        set
        {
            _ = corner switch
            {
                Corner.UpperLeft => UpperLeft = value,
                Corner.UpperRight => UpperRight = value,
                Corner.LowerLeft => LowerLeft = value,
                Corner.LowerRight => LowerRight = value,
                _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null),
            };
        }
    }
}