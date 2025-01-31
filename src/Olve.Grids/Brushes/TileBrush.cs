using System.Runtime.InteropServices;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct TileBrush(TileIndex TileIndex, Corner Corner, BrushId BrushId)
{
    public static implicit operator TileBrush((TileIndex tileIndex, Corner corner, BrushId brushId) value) =>
        new(value.tileIndex, value.corner, value.brushId);
}