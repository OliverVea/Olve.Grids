using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfiguration
{
    public bool GenerateFromBrushes { get; init; }
    public required IReadOnlyList<Adjacency> Adjacencies { get; init; }

    public class Adjacency
    {
        public required IReadOnlyCollection<TileIndex> Tiles { get; init; }
        public required Direction DirectionToOverwrite { get; init; }
        public required IReadOnlyList<Adjacent> Adjacents { get; init; }
    }

    public class Adjacent
    {
        public required IReadOnlyCollection<TileIndex> Tiles { get; init; }
        public bool IsAdjacent { get; init; }
        public required Direction Direction { get; init; }
    }
}