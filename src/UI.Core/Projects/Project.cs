using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
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
    public required HashSet<(TileIndex, Side)> LockedSides { get; init; }
    public required Dictionary<BrushId, ProjectBrush> Brushes { get; init; }
}