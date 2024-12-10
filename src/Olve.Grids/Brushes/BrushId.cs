namespace Olve.Grids.Brushes;

public readonly record struct BrushId(Id Id)
{
    public static BrushId New() => new(Id.NewId());
}