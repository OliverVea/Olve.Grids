using System.Collections.Frozen;
using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class FrozenWeightLookup(
    IEnumerable<KeyValuePair<TileIndex, float>> weights,
    float defaultValue = 0)
    : IReadOnlyWeightLookup
{
    private readonly FrozenDictionary<TileIndex, float> _weights = weights.ToFrozenDictionary();

    public float GetWeight(TileIndex tileIndex) => _weights.GetValueOrDefault(tileIndex, defaultValue);
    public IEnumerable<KeyValuePair<TileIndex, float>> Weights => _weights;
}