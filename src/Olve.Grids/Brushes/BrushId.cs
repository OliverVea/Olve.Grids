using System.Diagnostics;

namespace Olve.Grids.Brushes;

[DebuggerDisplay("{ToString}")]
public readonly record struct BrushId(string Value) : IComparable<BrushId>
{
    public override string ToString() => Value;
    public int CompareTo(BrushId other) => string.Compare(Value, other.Value, StringComparison.Ordinal);
}

[GenerateOneOf]
public partial class BrushIdOrAny : OneOfBase<BrushId, Any>
{
    public static readonly BrushIdOrAny Any = new Any();
}