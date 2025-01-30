using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Adjacencies;

public readonly record struct TileAdjacency(TileIndex From, TileIndex To, Direction Direction)
{
    public static implicit operator TileAdjacency((TileIndex from, TileIndex to, Direction direction) value) =>
        new(value.from, value.to, value.direction);
}