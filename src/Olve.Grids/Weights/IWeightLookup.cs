﻿using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public interface IWeightLookup : IReadOnlyWeightLookup
{
    void SetDefaultWeight(float newDefaultWeight);
    void SetWeight(TileIndex tileIndex, float weight);
    void ModifyWeight(TileIndex tileIndex, Func<float, float> modifier, float defaultValue = 1f);
}