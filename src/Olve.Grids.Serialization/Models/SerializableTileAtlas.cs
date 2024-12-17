using MemoryPack;
using Olve.Grids.Generation;
using Olve.Grids.Grids;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableTileAtlas
{
    public string FilePath { get; set; } = string.Empty;
    public SerializableGridConfiguration Grid { get; set; } = new();
    public SerializableAdjacencyLookup AdjacencyLookup { get; set; } = new();
    public SerializableBrushLookup BrushLookup { get; set; } = new();
    public SerializableWeightLookup WeightLookup { get; set; } = new();

    public int FallbackTile { get; set; }

    public static SerializableTileAtlas FromTileAtlas(TileAtlas tileAtlas) =>
        new()
        {
            FilePath = tileAtlas.FilePath,
            Grid = SerializableGridConfiguration.FromGridConfiguration(tileAtlas.Grid),
            AdjacencyLookup = SerializableAdjacencyLookup.FromAdjacencyLookup(tileAtlas.AdjacencyLookup),
            BrushLookup = SerializableBrushLookup.FromBrushLookup(tileAtlas.BrushLookup),
            WeightLookup = SerializableWeightLookup.FromWeightLookup(tileAtlas.WeightLookup),
            FallbackTile = tileAtlas.FallbackTile.Index,
        };

    public static TileAtlas ToTileAtlas(SerializableTileAtlas serializableTileAtlas) =>
        new(serializableTileAtlas.FilePath,
            SerializableGridConfiguration.ToGridConfiguration(serializableTileAtlas.Grid),
            SerializableBrushLookup.ToBrushLookup(serializableTileAtlas.BrushLookup),
            SerializableAdjacencyLookup.ToAdjacencyLookup(serializableTileAtlas.AdjacencyLookup),
            SerializableWeightLookup.ToWeightLookup(serializableTileAtlas.WeightLookup))
        {
            FallbackTile = new TileIndex(serializableTileAtlas.FallbackTile),
        };
}