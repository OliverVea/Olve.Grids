using System.Collections.Frozen;
using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class FrozenWeightLookup(TileWeights weights, float defaultWeight)
    : IReadOnlyWeightLookup
{
    private readonly FrozenDictionary<TileIndex, float> _weights =
        weights.ToFrozenDictionary(x => x.TileIndex, x => x.Weight);

    public float DefaultWeight { get; } = defaultWeight;
    public float GetWeight(TileIndex tileIndex) => _weights.GetValueOrDefault(tileIndex, DefaultWeight);
    public TileWeights Weights => TileWeights.FromEnumerable(_weights.Select(x => new TileWeight(x.Key, x.Value)));
}