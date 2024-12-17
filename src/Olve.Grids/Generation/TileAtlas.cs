using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation;

public class TileAtlas(
    string filePath,
    GridConfiguration gridConfiguration,
    IReadOnlyBrushLookup brushLookup,
    IReadOnlyAdjacencyLookup adjacencyLookup,
    IReadOnlyWeightLookup weightLookup
)
{
    public string FilePath { get; } = filePath;
    public GridConfiguration Grid { get; } = gridConfiguration;
    public IReadOnlyAdjacencyLookup AdjacencyLookup { get; } = adjacencyLookup;
    public IReadOnlyBrushLookup BrushLookup { get; } = brushLookup;
    public IReadOnlyWeightLookup WeightLookup { get; } = weightLookup;

    public TileIndex FallbackTile { get; init; } = new(gridConfiguration.TileCount - 1);
}