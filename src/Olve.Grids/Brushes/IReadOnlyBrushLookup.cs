using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public interface IReadOnlyBrushLookup
{
    /// <summary>
    /// Get the brush ids in the lookup
    /// </summary>
    IEnumerable<BrushId> Brushes { get; }

    /// <summary>
    /// Get the entries in the lookup
    /// </summary>
    TileBrushes TileBrushes { get; }

    /// <summary>
    /// Get the brushes for a tile
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    /// <returns>The brushes for the tile or NotFound if no brushes are found</returns>
    OneOf<CornerBrushes, NotFound> GetBrushes(TileIndex tileIndex);

    /// <summary>
    /// Get the brush id for a tile and corner
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    /// <param name="corner">The corner from the perspective of the tile</param>
    /// <returns>The brush id or NotFound if no brush id is found</returns>
    OneOf<BrushId, NotFound> GetBrushId(TileIndex tileIndex, Corner corner);

    /// <summary>
    /// Get the tiles that have a brush id set for the corner
    /// </summary>
    /// <param name="brushId">The brush id</param>
    /// <param name="corner">The corner <b>from the perspective of the brush</b></param>
    /// <returns>The tiles that have the brush id set for the corner or NotFound if no tiles are found</returns>
    /// <remarks>Please note that the corner is from the perspective of the brush, i.e., the opposite corner of the tile</remarks>
    OneOf<IReadOnlySet<TileIndex>, NotFound> GetTiles(BrushId brushId, Corner corner);
}