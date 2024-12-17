using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class WeightLookup(
    IEnumerable<KeyValuePair<TileIndex, float>>? weights = null,
    float defaultWeight = 1f) : IWeightLookup
{
    private Dictionary<TileIndex, float> Lookup { get; } =
        weights?.ToDictionary(pair => pair.Key, pair => pair.Value)
        ?? new Dictionary<TileIndex, float>();

    public float GetWeight(TileIndex tileIndex) => Lookup.GetValueOrDefault(tileIndex, defaultWeight);
    public IEnumerable<KeyValuePair<TileIndex, float>> Weights => Lookup;

    public void ModifyWeight(TileIndex tileIndex, Func<float, float> modifier, float defaultValue = 1f)
    {
        Lookup[tileIndex] = modifier(Lookup.GetValueOrDefault(tileIndex, defaultValue));
    }

    public void SetWeight(TileIndex tileIndex, float weight)
    {
        Lookup[tileIndex] = weight;
    }
}