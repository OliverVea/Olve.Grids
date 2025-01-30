using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class WeightLookup(
    TileWeights? weights = null,
    float defaultWeight = 1f) : IWeightLookup
{
    public WeightLookup(IEnumerable<TileIndex> tileIndices, float? weight = null, float defaultWeight = 1f)
        : this(TileWeights.FromEnumerable(
                tileIndices.Select(tileIndex => new TileWeight(tileIndex, weight ?? defaultWeight))),
            defaultWeight)
    {
    }

    private Dictionary<TileIndex, float> Lookup { get; } =
        weights?.ToDictionary(tileWeight => tileWeight.TileIndex, pair => pair.Weight)
        ?? new Dictionary<TileIndex, float>();

    public float DefaultWeight { get; private set; } = defaultWeight;
    public float GetWeight(TileIndex tileIndex) => Lookup.GetValueOrDefault(tileIndex, DefaultWeight);
    public TileWeights Weights => TileWeights.FromEnumerable(Lookup.Select(x => new TileWeight(x.Key, x.Value)));

    public void ModifyWeight(TileIndex tileIndex, Func<float, float> modifier, float defaultValue = 1f)
    {
        Lookup[tileIndex] = modifier(Lookup.GetValueOrDefault(tileIndex, defaultValue));
    }

    public void SetDefaultWeight(float newDefaultWeight)
    {
        DefaultWeight = DefaultWeight;
    }

    public void SetWeight(TileIndex tileIndex, float weight)
    {
        Lookup[tileIndex] = weight;
    }
}