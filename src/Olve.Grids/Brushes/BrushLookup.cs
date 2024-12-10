using Olve.Grids.Grids;
using Olve.Utilities.Collections;
using OneOf.Types;

namespace Olve.Grids.Brushes;

public class BrushLookup
{
    private readonly IManyToManyLookup<TileIndex, BrushId> _lookup = new ManyToManyLookup<TileIndex, BrushId>();
    
    public OneOf<IReadOnlySet<BrushId>, NotFound> GetBrushes(TileIndex tileIndex) => _lookup.Get(tileIndex);
    public void SetBrushes(TileIndex tileIndex, ISet<BrushId> brushes) => _lookup.Set(tileIndex, brushes);
    
    public OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId) => _lookup.Get(brushId);
    public void SetTiles(BrushId brushId, ISet<TileIndex> tiles) => _lookup.Set(brushId, tiles);

    public IEnumerable<KeyValuePair<TileIndex, BrushId>> Pairs => _lookup;
    
    public IEnumerable<TileIndex> TileIndices => _lookup.Lefts;
    public IEnumerable<BrushId> BrushIds => _lookup.Rights;
}
