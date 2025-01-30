using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public interface IReadOnlyWeightLookup
{
    float DefaultWeight { get; }

    TileWeights Weights { get; }
    float GetWeight(TileIndex tileIndex);
}