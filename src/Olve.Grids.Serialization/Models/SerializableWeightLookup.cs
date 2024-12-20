﻿using MemoryPack;
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

    public static IReadOnlyWeightLookup ToWeightLookup(SerializableWeightLookup serializableWeightLookup) =>
        new FrozenWeightLookup(serializableWeightLookup.Weights, serializableWeightLookup.DefaultWeight);

}