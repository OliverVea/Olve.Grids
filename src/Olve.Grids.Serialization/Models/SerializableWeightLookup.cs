using MemoryPack;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableWeightLookup
{
    public float DefaultWeight { get; set; }
    public IEnumerable<KeyValuePair<TileIndex, float>> Weights { get; set; } = [ ];

    public static SerializableWeightLookup FromWeightLookup(IReadOnlyWeightLookup weightLookup) =>
        new()
        {
            DefaultWeight = weightLookup.DefaultWeight,
            Weights = weightLookup.Weights,
        };

    public static FrozenWeightLookup ToFrozenWeightLookup(SerializableWeightLookup serializableWeightLookup) =>
        new(serializableWeightLookup.Weights, serializableWeightLookup.DefaultWeight);

    public static WeightLookup ToWeightLookup(SerializableWeightLookup serializableWeightLookup) =>
        new(serializableWeightLookup.Weights, serializableWeightLookup.DefaultWeight);
}