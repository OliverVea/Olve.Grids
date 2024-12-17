using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public interface IBrushLookup : IReadOnlyBrushLookup
{
    void SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes);

    void SetCornerBrush(
        TileIndex tileIndex,
        Corner corner,
        OneOf<BrushId, Any> brushId
    );

    void Clear();
    void ClearTileBrushes(TileIndex tileIndex);
    void ClearTileBrush(TileIndex tileIndex, Corner corner);
}