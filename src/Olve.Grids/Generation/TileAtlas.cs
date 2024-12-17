using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation;

public class TileAtlas(
    string filePath,
    GridConfiguration gridConfiguration,
    IReadOnlyBrushLookup readOnlyBrushLookup,
    IReadOnlyAdjacencyLookup readOnlyAdjacencyLookup,
    IWeightLookup weightLookup
)
{
    public string FilePath { get; } = filePath;
    public GridConfiguration Grid { get; } = gridConfiguration;
    public IReadOnlyAdjacencyLookup ReadOnlyAdjacencyLookup { get; } = readOnlyAdjacencyLookup;
    public IReadOnlyBrushLookup ReadOnlyBrushLookup { get; } = readOnlyBrushLookup;
    public IWeightLookup WeightLookup { get; } = weightLookup;

    public TileIndex FallbackTile { get; init; } = new(gridConfiguration.TileCount - 1);
}