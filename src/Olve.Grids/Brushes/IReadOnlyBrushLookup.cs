using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public interface IReadOnlyBrushLookup
{
    IEnumerable<BrushId> Brushes { get; }
    OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner);
    OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner);

    IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> Entries { get; }
}