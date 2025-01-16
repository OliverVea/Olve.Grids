using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.IO.TileAtlasBuilder;

public class TileAtlasConfiguration
{
    public Size? TileSize { get; internal set; }
    public Size? ImageSize { get; internal set; }
    public int? Columns { get; internal set; }
    public int? Rows { get; internal set; }
    public TileIndex? FallbackTileIndex { get; internal set; }

    public IAdjacencyLookup AdjacencyLookup { get; internal set; } = new AdjacencyLookup();
    public IBrushLookup BrushLookup { get; internal set; } = new BrushLookup();
    public IWeightLookup WeightLookup { get; internal set; } = new WeightLookup();
}