using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation;

public class TileAtlas(
    GridConfiguration gridConfiguration,
    FrozenBrushLookup brushLookup,
    FrozenAdjacencyLookup adjacencyLookup,
    FrozenWeightLookup weightLookup)
{
    public GridConfiguration Grid { get; } = gridConfiguration;
    public FrozenAdjacencyLookup AdjacencyLookup { get; } = adjacencyLookup;
    public FrozenBrushLookup BrushLookup { get; } = brushLookup;
    public FrozenWeightLookup WeightLookup { get; } = weightLookup;

    public TileIndex FallbackTile { get; init; } = new(gridConfiguration.TileCount - 1);
}