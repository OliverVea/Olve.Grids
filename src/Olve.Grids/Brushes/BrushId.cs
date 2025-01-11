using System.Diagnostics;

namespace Olve.Grids.Brushes;

[DebuggerDisplay("{ShortDisplay}")]
public readonly record struct BrushId(Id Id, string DisplayName) : IComparable<BrushId>
{
    private string ShortDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : DisplayName;
    private string LongDisplay => string.IsNullOrWhiteSpace(DisplayName) ? Id.ToString() : $"{Id} ({DisplayName})";

    public static BrushId New(string displayName) => new(Id.NewId(), displayName);

    public override string ToString() => ShortDisplay;

    public int CompareTo(BrushId other) => Id.CompareTo(other.Id);
}

[GenerateOneOf]
public partial class BrushIdOrAny : OneOfBase<BrushId, Any>
{
    public static readonly BrushIdOrAny Any = new Any();
}