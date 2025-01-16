using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableBrushLookup
{
    public SerializableTileBrush[] Entries { get; set; } = [ ];

    public static SerializableBrushLookup FromBrushLookup(IReadOnlyBrushLookup brushLookup) =>
        new()
        {
            Entries = brushLookup
                .Entries.Select(SerializableTileBrush.FromTileBrush)
                .ToArray(),
        };

    private IEnumerable<(TileIndex, Corner, BrushId)> Items => Entries.Select(x => x.ToTileBrush());

    public FrozenBrushLookup ToFrozenBrushLookup() => new(Items);
    public BrushLookup ToBrushLookup() => new(Items);
}