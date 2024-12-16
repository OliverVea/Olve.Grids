namespace Olve.Grids.Grids;

public readonly record struct TileIndex(int Index)
{
    public static TileIndex operator ++(TileIndex index) => new(index.Index + 1);
}