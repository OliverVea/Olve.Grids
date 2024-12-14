using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public interface IWeightLookup : IEnumerable<KeyValuePair<TileIndex, float>>
{
    float GetWeight(TileIndex tileIndex);
}