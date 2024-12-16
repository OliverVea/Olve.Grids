namespace Olve.Grids.Grids;

public readonly record struct TileIndex(int Index)
{
    public static TileIndex operator ++(TileIndex index)
    {
        return new TileIndex(index.Index + 1);
    }
}