using System.Diagnostics;

namespace Olve.Grids.Brushes;

[DebuggerDisplay("{ToString()}")]
public readonly struct BrushId(string? value) : IComparable<BrushId>, IEquatable<BrushId>
{
    public string? Value { get; } = value;

    public override string ToString() => Value ?? string.Empty;

    public int CompareTo(BrushId other) => string.Compare(Value, other.Value, StringComparison.Ordinal);
    public static bool operator <(BrushId a, BrushId b) => a.CompareTo(b) < 0;
    public static bool operator <=(BrushId a, BrushId b) => a.CompareTo(b) <= 0;
    public static bool operator >(BrushId a, BrushId b) => a.CompareTo(b) > 0;
    public static bool operator >=(BrushId a, BrushId b) => a.CompareTo(b) >= 0;
    public static bool operator ==(BrushId a, BrushId b) => a.CompareTo(b) == 0;
    public static bool operator !=(BrushId a, BrushId b) => a.CompareTo(b) != 0;

    public override bool Equals(object? obj) => obj is BrushId other && Equals(other);
    public override int GetHashCode() => Value == null ? 0 : StringComparer.Ordinal.GetHashCode(Value);

    public bool Equals(BrushId other) => string.Equals(Value, other.Value, StringComparison.Ordinal);
}