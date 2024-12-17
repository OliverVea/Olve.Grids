using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public record CornerBrushes(
    OneOf<BrushId, Any> UpperLeft,
    OneOf<BrushId, Any> UpperRight,
    OneOf<BrushId, Any> LowerLeft,
    OneOf<BrushId, Any> LowerRight)
{
    public static readonly CornerBrushes Any = new(new Any(), new Any(), new Any(), new Any());

    public OneOf<BrushId, Any> this[Corner corner] => corner switch
    {
        Corner.UpperLeft => UpperLeft,
        Corner.UpperRight => UpperRight,
        Corner.LowerLeft => LowerLeft,
        Corner.LowerRight => LowerRight,
        _ => throw new ArgumentOutOfRangeException(nameof(corner), corner, null),
    };
}