using DeBroglie;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Utilities.Types;
using static Olve.Grids.Generation.Sandbox.TileIndices;
using static Olve.Grids.Generation.Sandbox.BrushIds;

namespace Olve.Grids.Generation.Sandbox;

public static class TileBrushes
{
    public record CornerBrushesForTile(
        TileIndex TileIndex,
        OneOf.OneOf<BrushId, Any> UpperLeft,
        OneOf.OneOf<BrushId, Any> UpperRight,
        OneOf.OneOf<BrushId, Any> LowerLeft,
        OneOf.OneOf<BrushId, Any> LowerRight)
    {
        public void Deconstruct(out TileIndex tileIndex, out CornerBrushes cornerBrushes)
        {
            tileIndex = TileIndex;
            cornerBrushes = new CornerBrushes(UpperLeft, UpperRight, LowerLeft, LowerRight);
        }
    }
    
    public static readonly CornerBrushesForTile I0Brush = new(I0, Dark, Dark, Dark, Light);
    public static readonly CornerBrushesForTile I1Brush = new(I1, Dark, Dark, Light, Light);
    public static readonly CornerBrushesForTile I2Brush = new(I2, Dark, Dark, Light, Dark);
    public static readonly CornerBrushesForTile I3Brush = new(I3, Light, Light, Light, Dark);
    public static readonly CornerBrushesForTile I4Brush = new(I4, Light, Light, Dark, Light);
    public static readonly CornerBrushesForTile I5Brush = new(I5, Dark, Light, Dark, Light);
    public static readonly CornerBrushesForTile I6Brush = new(I6, Light, Light, Light, Light);
    public static readonly CornerBrushesForTile I7Brush = new(I7, Light, Dark, Light, Dark);
    public static readonly CornerBrushesForTile I8Brush = new(I8, Light, Dark, Light, Light);
    public static readonly CornerBrushesForTile I9Brush = new(I9, Dark, Light, Light, Light);
    public static readonly CornerBrushesForTile I10Brush = new(I10, Dark, Light, Dark, Dark);
    public static readonly CornerBrushesForTile I11Brush = new(I11, Light, Light, Dark, Dark);
    public static readonly CornerBrushesForTile I12Brush = new(I12, Light, Dark, Dark, Dark);
    public static readonly CornerBrushesForTile I13Brush = new(I13, Dark, Dark, Dark, Dark);
    
    public static IReadOnlyList<CornerBrushesForTile> AllBrushes =
    [
        I0Brush,
        I1Brush,
        I2Brush,
        I3Brush,
        I4Brush,
        I5Brush,
        I6Brush,
        I7Brush,
        I8Brush,
        I9Brush,
        I10Brush,
        I11Brush,
        I12Brush,
        I13Brush
    ];
}