using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableBrushLookup
{
    public IEnumerable<(int TileIndex, int Corner, string BrushId)> Entries { get; set; } = [ ];

    public static SerializableBrushLookup FromBrushLookup(IReadOnlyBrushLookup brushLookup) =>
        new()
        {
            Entries = brushLookup.Entries.Select(x =>
                (
                    x.TileIndex.Index,
                    (int)x.Corner, x.Brush.Value)
            ),
        };

    private IEnumerable<(TileIndex, Corner, BrushId)> Items => Entries.Select(x =>
    (
        new TileIndex(x.TileIndex),
        (Corner)x.Corner,
        new BrushId(x.BrushId)
    ));


    public static FrozenBrushLookup ToFrozenBrushLookup(SerializableBrushLookup serializableBrushLookup) =>
        new(serializableBrushLookup.Items);

    public static BrushLookup ToBrushLookup(SerializableBrushLookup serializableBrushLookup) =>
        new(serializableBrushLookup.Items);
}