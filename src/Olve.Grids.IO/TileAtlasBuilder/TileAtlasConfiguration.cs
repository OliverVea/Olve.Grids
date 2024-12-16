using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.IO.TileAtlasBuilder;

public class TileAtlasConfiguration
{
    public string? FilePath { get; internal set; }
    public Size? ImageSize { get; internal set; }
    public Size? TileSize { get; internal set; }
    public int? Columns { get; internal set; }
    public int? Rows { get; internal set; }
    public TileIndex? FallbackTileIndex { get; internal set; }

    public IAdjacencyLookupBuilder? AdjacencyLookupBuilder { get; internal set; }
    public IBrushLookupBuilder? BrushLookupBuilder { get; internal set; }
    public IWeightLookupBuilder? WeightLookupBuilder { get; internal set; }
}