using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public interface IWeightLookupBuilder
{
    IWeightLookupBuilder SetWeight(TileIndex tileIndex, float weight);
    float GetWeight(TileIndex tileIndex);
    IWeightLookup Build();
}