using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public interface IReadOnlyWeightLookup
{
    float GetWeight(TileIndex tileIndex);

    IEnumerable<KeyValuePair<TileIndex, float>> Weights { get; }
}