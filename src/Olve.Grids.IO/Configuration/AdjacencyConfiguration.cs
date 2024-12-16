using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfiguration
{
    public bool GenerateFromBrushes { get; init; }
    public required IReadOnlyList<Adjacency> Adjacencies { get; init; }

    public class Adjacency
    {
        public required TileIndex Tile { get; init; }
        public required AdjacencyDirection AdjacencyDirectionToOverwrite { get; init; }
        public required IReadOnlyList<Adjacent> Adjacents { get; init; }
    }

    public class Adjacent
    {
        public required TileIndex Tile { get; init; }
        public bool IsAdjacent { get; init; }
        public required AdjacencyDirection Direction { get; init; }
    }
}
