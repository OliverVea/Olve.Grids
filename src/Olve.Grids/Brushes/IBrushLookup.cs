using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Brushes;

public interface IBrushLookup : IReadOnlyBrushLookup
{
    /// <summary>
    /// Set the brushes for a tile
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    /// <param name="cornerBrushes">The brushes for the tile</param>
    void SetCornerBrushes(TileIndex tileIndex, CornerBrushes cornerBrushes);

    /// <summary>
    /// Set the brush id for a tile and corner
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    /// <param name="corner">The corner from the perspective of the tile</param>
    /// <param name="brushId">The brush id</param>
    void SetCornerBrush(
        TileIndex tileIndex,
        Corner corner,
        BrushIdOrAny brushId
    );

    /// <summary>
    /// Clear all brushes
    /// </summary>
    void Clear();

    /// <summary>
    /// Clear all brushes for a tile
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    void ClearTileBrushes(TileIndex tileIndex);

    /// <summary>
    /// Clear the brush id for a tile and corner
    /// </summary>
    /// <param name="tileIndex">The tile index</param>
    /// <param name="corner">The corner from the perspective of the tile</param>
    void ClearTileBrush(TileIndex tileIndex, Corner corner);
}