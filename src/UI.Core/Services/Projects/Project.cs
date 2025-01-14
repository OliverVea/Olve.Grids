using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Utilities.Lookup;

namespace UI.Core.Services.Projects;

public record Project(
    Id<Project> Id,
    ProjectName Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastAccessedAt,
    FileContent TileSheetImage,
    HashSet<TileIndex> ActiveTiles,
    Dictionary<BrushId, ProjectBrush> Brushes,
    TileAtlasBuilder TileAtlasBuilder)
    : IHasId<Id<Project>>;

public readonly record struct ProjectBrush(BrushId Id, string DisplayName, ColorString Color);

[GenerateOneOf]
public partial class ProjectBrushOrAny : OneOfBase<ProjectBrush, Any>
{
    public static readonly ProjectBrushOrAny Any = new Any();
}