using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation;

public class TileAtlas(
    string filePath,
    GridConfiguration gridConfiguration,
    IBrushLookup brushLookup,
    IAdjacencyLookup adjacencyLookup,
    IWeightLookup weightLookup
)
{
    public string FilePath { get; } = filePath;
    public GridConfiguration Grid { get; } = gridConfiguration;
    public IAdjacencyLookup AdjacencyLookup { get; } = adjacencyLookup;
    public IBrushLookup BrushLookup { get; } = brushLookup;
    public IWeightLookup WeightLookup { get; } = weightLookup;

    public TileIndex FallbackTile { get; init; } = new(gridConfiguration.TileCount - 1);
}