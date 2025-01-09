using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public record CornerBrushes
{
    public OneOf<BrushId, Any> UpperLeft { get; set; } = new Any();
    public OneOf<BrushId, Any> UpperRight { get; set; } = new Any();
    public OneOf<BrushId, Any> LowerLeft { get; set; } = new Any();
    public OneOf<BrushId, Any> LowerRight { get; set; } = new Any();

    public OneOf<BrushId, Any> this[Corner corner]
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