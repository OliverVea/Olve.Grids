using System.Collections;
using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class WeightLookup(
    IEnumerable<KeyValuePair<TileIndex, float>>? weights = null,
    float defaultWeight = 1f
) : IWeightLookup, IWeightLookupBuilder, IEnumerable<KeyValuePair<TileIndex, float>>
{
    private Dictionary<TileIndex, float> Lookup { get; } =
        weights?.ToDictionary(pair => pair.Key, pair => pair.Value)
        ?? new Dictionary<TileIndex, float>();

    public WeightLookup(IEnumerable<TileIndex> tileIndices, float defaultWeight = 1f)
        : this(
            tileIndices.Select(x => new KeyValuePair<TileIndex, float>(x, defaultWeight)),
            defaultWeight
        )
    { }

    public IEnumerator<KeyValuePair<TileIndex, float>> GetEnumerator()
    {
        return Lookup.GetEnumerator();
    }

    public float GetWeight(TileIndex tileIndex)
    {
        return Lookup.GetValueOrDefault(tileIndex, defaultWeight);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public IWeightLookupBuilder SetWeight(TileIndex tileIndex, float weight)
    {
        Lookup[tileIndex] = weight;

        return this;
    }

    public IWeightLookup Build()
    {
        return this;
    }
}