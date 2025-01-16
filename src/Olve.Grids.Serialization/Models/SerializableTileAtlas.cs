using MemoryPack;
using Olve.Grids.Generation;
using Olve.Grids.Grids;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableTileAtlas
{
    public SerializableGridConfiguration Grid { get; set; } = new();
    public SerializableAdjacencyLookup AdjacencyLookup { get; set; } = new();
    public SerializableBrushLookup BrushLookup { get; set; } = new();
    public SerializableWeightLookup WeightLookup { get; set; } = new();

    public int FallbackTile { get; set; }

    public static SerializableTileAtlas FromTileAtlas(TileAtlas tileAtlas) =>
        new()
        {
            Grid = SerializableGridConfiguration.FromGridConfiguration(tileAtlas.Grid),
            AdjacencyLookup = SerializableAdjacencyLookup.FromAdjacencyLookup(tileAtlas.AdjacencyLookup),
            BrushLookup = SerializableBrushLookup.FromBrushLookup(tileAtlas.BrushLookup),
            WeightLookup = SerializableWeightLookup.FromWeightLookup(tileAtlas.WeightLookup),
            FallbackTile = tileAtlas.FallbackTile.Index,
        };

    public static TileAtlas ToTileAtlas(SerializableTileAtlas serializableTileAtlas) =>
        new(serializableTileAtlas.Grid.ToGridConfiguration(),
            serializableTileAtlas.BrushLookup.ToFrozenBrushLookup(),
            serializableTileAtlas.AdjacencyLookup.ToFrozenAdjacencyLookup(),
            serializableTileAtlas.WeightLookup.ToFrozenWeightLookup())
        {
            FallbackTile = new TileIndex(serializableTileAtlas.FallbackTile),
        };
}