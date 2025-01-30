using MemoryPack;
using Olve.Grids.Brushes;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableBrushLookup
{
    public SerializableTileBrush[] Entries { get; set; } = [ ];

    public static SerializableBrushLookup FromBrushLookup(IReadOnlyBrushLookup brushLookup) =>
        new()
        {
            Entries = brushLookup
                .TileBrushes.Select(SerializableTileBrush.FromTileBrush)
                .ToArray(),
        };

    private TileBrushes Items => TileBrushes.FromEnumerable(Entries.Select(x => x.ToTileBrush()));

    public FrozenBrushLookup ToFrozenBrushLookup() => new(Items);
    public BrushLookup ToBrushLookup() => new(Items);
}