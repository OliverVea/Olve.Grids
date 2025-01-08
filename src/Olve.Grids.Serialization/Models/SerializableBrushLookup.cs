using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableBrushLookup
{
    public IEnumerable<(int TileIndex, int Corner, (Ulid Id, string DisplayName) Brush)> Entries { get; set; } = [ ];

    public static SerializableBrushLookup FromBrushLookup(IReadOnlyBrushLookup brushLookup) =>
        new()
        {
            Entries = brushLookup.Entries.Select(x =>
            (
                x.TileIndex.Index,
                (int)x.Corner, (
                    x.Brush.Id.Value,
                    x.Brush.DisplayName))),
        };

    private IEnumerable<(TileIndex, Corner, BrushId)> Items => Entries.Select(x =>
    (
        new TileIndex(x.TileIndex),
        (Corner)x.Corner,
        new BrushId(new Id(x.Brush.Id), x.Brush.DisplayName)
    ));


    public static FrozenBrushLookup ToFrozenBrushLookup(SerializableBrushLookup serializableBrushLookup) =>
        new(serializableBrushLookup.Items);

    public static BrushLookup ToBrushLookup(SerializableBrushLookup serializableBrushLookup) =>
        new(serializableBrushLookup.Items);
}