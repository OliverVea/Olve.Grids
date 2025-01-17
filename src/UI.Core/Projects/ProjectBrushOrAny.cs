using Olve.Grids.Brushes;

namespace UI.Core.Projects;

[GenerateOneOf]
public partial class ProjectBrushOrAny : OneOfBase<ProjectBrush, Any>, IEquatable<ProjectBrushOrAny>
{
    public BrushIdOrAny Id => Match<BrushIdOrAny>(x => x.Id, x => x);
    public static readonly ProjectBrushOrAny Any = new Any();

    public static implicit operator BrushIdOrAny(ProjectBrushOrAny value) => value.Id;

    public bool Equals(ProjectBrushOrAny? other)
    {
        if (other is null)
        {
            return false;
        }

        if (other.TryPickT1(out _, out var otherBrush))
        {
            return IsT1;
        }

        return TryPickT0(out var brush, out _) && brush.Id.Equals(otherBrush.Id);
    }

    public override int GetHashCode() => TryPickT0(out var brush, out _) ? brush.GetHashCode() : 0;
}