using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.IO.TileAtlasBuilder;

public class TileAtlasConfiguration
{
    public string? FilePath { get; internal set; }
    public Size? TileSize { get; internal set; }
    public int? Columns { get; internal set; }
    public int? Rows { get; internal set; }
    public TileIndex? FallbackTileIndex { get; internal set; }

    public IAdjacencyLookup? AdjacencyLookup { get; internal set; }
    public IBrushLookup? BrushLookup { get; internal set; }
    public IWeightLookup? WeightLookup { get; internal set; }
}