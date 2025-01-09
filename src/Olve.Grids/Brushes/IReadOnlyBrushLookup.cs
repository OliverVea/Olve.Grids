using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public interface IReadOnlyBrushLookup
{
    IEnumerable<BrushId> Brushes { get; }
    OneOf<CornerBrushes, NotFound> GetBrushes(TileIndex tileIndex);
    OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner);
    OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner);

    IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId Brush)> Entries { get; }
}