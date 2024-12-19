using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation;

public class TileAtlas(
    string filePath,
    GridConfiguration gridConfiguration,
    FrozenBrushLookup brushLookup,
    FrozenAdjacencyLookup adjacencyLookup,
    FrozenWeightLookup weightLookup)
{
    public string FilePath { get; } = filePath;
    public GridConfiguration Grid { get; } = gridConfiguration;
    public FrozenAdjacencyLookup AdjacencyLookup { get; } = adjacencyLookup;
    public FrozenBrushLookup BrushLookup { get; } = brushLookup;
    public FrozenWeightLookup WeightLookup { get; } = weightLookup;

    public TileIndex FallbackTile { get; init; } = new(gridConfiguration.TileCount - 1);
}