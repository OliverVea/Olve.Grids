using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration;

public class WeightConfiguration
{
    public required IReadOnlyList<TileWeight> Weights { get; init; }

    public class TileWeight
    {
        public required IReadOnlyCollection<TileIndex> Tiles { get; init; }
        public required Func<float, float> WeightFunction { get; init; }
    }
}