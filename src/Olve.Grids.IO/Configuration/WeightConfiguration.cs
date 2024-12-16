using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration;

public class WeightConfiguration
{
    public required IReadOnlyList<TileWeight> Weights { get; init; }

    public class TileWeight
    {
        public required TileIndex Tile { get; init; }
        public required float Weight { get; init; }
    }
}
