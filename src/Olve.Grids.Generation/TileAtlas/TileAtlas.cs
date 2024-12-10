using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Generation.TileAtlas;

public class TileAtlas(string filePath, GridConfiguration gridConfiguration)
{
    public string FilePath { get; } = filePath;
    public GridConfiguration Grid { get; } = gridConfiguration;

    /// <summary>
    /// The tile to use when contradictory Brush and Adjacency rules are found.
    /// Defaults to the last tile in the atlas.
    /// </summary>
    public TileIndex FallbackTileIndex { get; init; } = new(gridConfiguration.TileCount - 1);

    public AdjacencyLookup AdjacencyLookup { get; } = new();
    public BrushLookup BrushLookup { get; } = new();
    public WeightLookup WeightLookup { get; } = new(gridConfiguration.GetTileIndices());
}
