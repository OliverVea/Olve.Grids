using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfiguration
{
    public bool GenerateFromBrushes { get; set; }
    public required IReadOnlyList<Adjacency> Adjacencies { get; set; }
    
    public class Adjacency
    {
        public TileIndex Tile { get; set; }
        public IReadOnlyList<Adjacent> Adjacents { get; set; }
    }

    public class Adjacent
    {
        public required TileIndex Tile { get; set; }
        public bool IsAdjacent { get; set; }
        public required AdjacencyDirection Direction { get; set; }
        
    }
}
