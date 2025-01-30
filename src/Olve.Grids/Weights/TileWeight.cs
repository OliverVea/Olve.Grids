using Olve.Grids.Grids;

namespace Olve.Grids.Weights;

public readonly record struct TileWeight(TileIndex TileIndex, float Weight)
{
    public static implicit operator TileWeight((TileIndex tileIndex, float weight) value) =>
        new(value.tileIndex, value.weight);
}