using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;
using Olve.Utilities.Lookup;

namespace UI.Core.Projects;

public record Project : IHasId<Id<Project>>
{
    public required Id<Project> Id { get; init; }
    public required ProjectName Name { get; init; }
    public required DateTimeOffset CreatedAt { get; init; }
    public required DateTimeOffset LastAccessedAt { get; init; }
    public required FileContent TileSheetImage { get; init; }
    public required GridConfiguration GridConfiguration { get; init; }
    public required IWeightLookup WeightLookup { get; init; }
    public required IAdjacencyLookup AdjacencyLookup { get; init; }
    public required IBrushLookup BrushLookup { get; init; }
    public required HashSet<TileIndex> ActiveTiles { get; init; }
    public required Dictionary<BrushId, ProjectBrush> Brushes { get; init; }
}

public readonly record struct ProjectBrush(BrushId Id, string DisplayName, ColorString Color);

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