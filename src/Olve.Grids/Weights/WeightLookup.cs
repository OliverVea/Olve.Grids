﻿using System.Collections;
using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public class WeightLookup(IEnumerable<KeyValuePair<TileIndex, float>> weights) : IEnumerable<KeyValuePair<TileIndex, float>>
{
    private Dictionary<TileIndex, float> Lookup { get; } = weights.ToDictionary(pair => pair.Key, pair => pair.Value);
    
    public WeightLookup(IEnumerable<TileIndex> tileIndices, float defaultWeight = 1f) : this(tileIndices.Select(x => new KeyValuePair<TileIndex, float>(x, defaultWeight)))
    {
        
    }
    
    public float this[TileIndex tileIndex]
    {
        get => Lookup.GetValueOrDefault(tileIndex, 1f);
        set => Lookup[tileIndex] = value;
    }

    public IEnumerator<KeyValuePair<TileIndex, float>> GetEnumerator()
    {
        return Lookup.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}