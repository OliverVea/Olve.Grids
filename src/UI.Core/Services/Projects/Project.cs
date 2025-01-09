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
    TileAtlasBuilder TileAtlasBuilder)
    : IHasId<Id<Project>>;