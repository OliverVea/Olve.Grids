using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableTileBrush
{
    public required int TileIndex { get; init; }
    public required Corner Corner { get; init; }
    public required string BrushId { get; init; }

    public static SerializableTileBrush FromTileBrush(TileBrush tileBrush) =>
        new()
        {
            TileIndex = tileBrush.TileIndex.Index,
            Corner = tileBrush.Corner,
            BrushId = tileBrush.BrushId.Value,
        };

    public TileBrush ToTileBrush() =>
        (new TileIndex(TileIndex), Corner, new BrushId(BrushId));
}