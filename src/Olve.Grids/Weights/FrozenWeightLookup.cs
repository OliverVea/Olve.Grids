using System.Collections.Frozen;
using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class FrozenWeightLookup(IEnumerable<KeyValuePair<TileIndex, float>> weights, float defaultWeight)
    : IReadOnlyWeightLookup
{
    private readonly FrozenDictionary<TileIndex, float> _weights = weights.ToFrozenDictionary();

    public float DefaultWeight { get; } = defaultWeight;
    public float GetWeight(TileIndex tileIndex) => _weights.GetValueOrDefault(tileIndex, DefaultWeight);
    public IEnumerable<KeyValuePair<TileIndex, float>> Weights => _weights;
}