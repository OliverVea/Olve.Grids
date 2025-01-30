using MemoryPack;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableWeightLookup
{
    public float DefaultWeight { get; set; }
    public IEnumerable<KeyValuePair<TileIndex, float>> Weights { get; set; } = [ ];
    private TileWeights TileWeights => TileWeights.FromEnumerable(Weights.Select(x => new TileWeight(x.Key, x.Value)));

    public static SerializableWeightLookup FromWeightLookup(IReadOnlyWeightLookup weightLookup) =>
        new()
        {
            DefaultWeight = weightLookup.DefaultWeight,
            Weights = weightLookup.Weights.Select(x => new KeyValuePair<TileIndex, float>(x.TileIndex, x.Weight)),
        };

    public FrozenWeightLookup ToFrozenWeightLookup() => new(TileWeights, DefaultWeight);

    public WeightLookup ToWeightLookup() => new(TileWeights, DefaultWeight);
}