using Olve.Grids.Grids;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public interface IBrushLookupBuilder
{
    OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner);
    IBrushLookupBuilder SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes);
    IBrushLookupBuilder SetCornerBrush(
        TileIndex tileIndex,
        Corner corner,
        OneOf<BrushId, Any> brushId
    );
    IBrushLookupBuilder Clear();
    IBrushLookupBuilder ClearTileBrushes(TileIndex tileIndex);
    IBrushLookupBuilder ClearTileBrush(TileIndex tileIndex, Corner corner);
    IBrushLookup Build();
}
