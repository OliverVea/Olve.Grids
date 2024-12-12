using System.Diagnostics;

namespace Olve.Grids.Brushes;

[DebuggerDisplay("{ShortDisplay}")]
public readonly record struct BrushId(Id Id, string DisplayName)
{
    public static BrushId New() => new(Id.NewId(), string.Empty);
    public static BrushId New(string displayName) => new(Id.NewId(), displayName);

    public string ShortDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : DisplayName;
    public string LongDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : $"{Id} ({DisplayName})";

    public override string ToString() => ShortDisplay;
}