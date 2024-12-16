using Olve.Grids.Grids;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public interface IBrushLookup : IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)>
{
    OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner);
    OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner);
    IEnumerable<BrushId> Brushes { get; }
}