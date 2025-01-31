using System.Diagnostics;

namespace Olve.Grids.Grids;

[DebuggerDisplay("Tile{Index}")]
public readonly record struct TileIndex(int Index)
{
    public static TileIndex operator ++(TileIndex index) => new(index.Index + 1);

    public static IEnumerable<TileIndex> Span(TileIndex start, TileIndex end)
    {
        if (start.Index > end.Index)
        {
            throw new ArgumentException("Start index must be less than or equal to end index.");
        }

        for (var i = start.Index; i <= end.Index; i++)
        {
            yield return new TileIndex(i);
        }
    }
}