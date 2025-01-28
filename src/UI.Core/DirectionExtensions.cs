using Olve.Grids.Primitives;

namespace UI.Core;

public static class DirectionExtensions
{
    public static IconDescriptor ToChevron(this Direction direction) => direction switch
    {
        Direction.Down => IconDescriptor.ChevronDown,
        Direction.Up => IconDescriptor.ChevronUp,
        Direction.Left => IconDescriptor.ChevronLeft,
        Direction.Right => IconDescriptor.ChevronRight,
        _ => throw new ArgumentOutOfRangeException(nameof(Direction), direction, null),
    };
}