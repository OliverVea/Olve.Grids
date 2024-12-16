using System.Diagnostics;

namespace Olve.Grids.Brushes;

[DebuggerDisplay("{ShortDisplay}")]
public readonly record struct BrushId(Id Id, string DisplayName)
{

    public string ShortDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : DisplayName;
    public string LongDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : $"{Id} ({DisplayName})";

    public static BrushId New()
    {
        return new BrushId(Id.NewId(), string.Empty);
    }

    public static BrushId New(string displayName)
    {
        return new BrushId(Id.NewId(), displayName);
    }

    public override string ToString() => ShortDisplay;
}