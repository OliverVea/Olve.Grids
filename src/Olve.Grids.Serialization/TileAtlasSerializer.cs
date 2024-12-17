using MemoryPack;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Grids.Weights;
using Olve.Utilities.Assertions;

namespace Olve.Grids.Serialization;

public class TileAtlasSerializer
{
    public byte[] Serialize(TileAtlas tileAtlas)
    {
        var serializableTileAtlas = Models.SerializableTileAtlas.FromTileAtlas(tileAtlas);

        return MemoryPackSerializer.Serialize(serializableTileAtlas);
    }

    public TileAtlas? Deserialize(byte[] data)
    {
        var serializableTileAtlas = MemoryPackSerializer.Deserialize<Models.SerializableTileAtlas>(data);
        if (serializableTileAtlas == null)
        {
            return null;
        }

        return Models.SerializableTileAtlas.ToTileAtlas(serializableTileAtlas);
    }
}

public static class Program
{
    // Tile constants
    private static readonly TileIndex BrushTile1 = new(42);
    private static readonly TileIndex BrushTile2 = new(43);

    private static readonly TileIndex AdjacencyTile1 = new(69);
    private static readonly TileIndex AdjacencyTile2 = new(70);
    private static readonly TileIndex AdjacencyTile3 = new(71);

    private static readonly TileIndex WeightTile1 = new(42);
    private static readonly TileIndex WeightTile2 = new(43);

    // Grid configuration
    private static readonly Size GridSize = new(4, 4);
    private const int TileRows = 3;
    private const int TileColumns = 4;

    // Weights
    private const float Weight1 = 0.5f;
    private const float Weight2 = 0.75f;

    // Brush display names
    private const string BrushDisplayName1 = "Hi!";
    private const string BrushDisplayName2 = "Hello!";

    // Path
    private const string TileAtlasPath = "AABBABAB";

    public static void Main()
    {
        var gridConfiguration = new GridConfiguration(GridSize, TileRows, TileColumns);

        var brushLookup = new BrushLookup();
        brushLookup.SetCornerBrush(BrushTile1, Corner.UpperLeft, BrushId.New(BrushDisplayName1));
        brushLookup.SetCornerBrush(BrushTile2, Corner.UpperRight, BrushId.New(BrushDisplayName2));
        var frozenBrushLookup = new FrozenBrushLookup(brushLookup.Entries);

        var adjacencyLookup = new AdjacencyLookup();
        adjacencyLookup.Set(AdjacencyTile1, AdjacencyTile1, Direction.Right);
        adjacencyLookup.Set(AdjacencyTile2, AdjacencyTile3, Direction.Down);
        var frozenAdjacencyLookup = new FrozenAdjacencyLookup(adjacencyLookup.Adjacencies);

        var weightLookup = new WeightLookup();
        weightLookup.SetWeight(WeightTile1, Weight1);
        weightLookup.SetWeight(WeightTile2, Weight2);
        var frozenWeightLookup = new FrozenWeightLookup(weightLookup.Weights);

        var tileAtlas = new TileAtlas(TileAtlasPath,
            gridConfiguration,
            frozenBrushLookup,
            frozenAdjacencyLookup,
            frozenWeightLookup);

        var serializer = new TileAtlasSerializer();

        var data = serializer.Serialize(tileAtlas);

        var deserializedTileAtlas = serializer.Deserialize(data);

        Assert.That(() => deserializedTileAtlas != null, "Deserialized tile atlas is null");
        Assert.That(() => deserializedTileAtlas!.FilePath == TileAtlasPath, "File path is not equal");

        // Verify Grid Configuration
        Assert.That(() => deserializedTileAtlas.Grid.TileCount == gridConfiguration.TileCount,
            "Tile count mismatch in GridConfiguration");

        foreach (var tile in gridConfiguration.GetTileIndices())
        {
            var originalIndex = gridConfiguration.GetRowAndColumn(tile);
            var deserializedIndex = deserializedTileAtlas.Grid.GetRowAndColumn(tile);

            Assert.That(() => originalIndex == deserializedIndex,
                $"Grid row/column mismatch for tile {tile.Index}");
        }

        // Verify Brush Lookup
        foreach (var entry in frozenBrushLookup.Entries)
        {
            var deserializedBrush = deserializedTileAtlas.BrushLookup.GetBrushId(entry.TileIndex, entry.Corner);
            Assert.That(() =>
                    deserializedBrush.Match(x => x.Id == entry.Brush.Id && x.DisplayName == entry.Brush.DisplayName,
                        x => false),
                $"Brush lookup mismatch for Tile {entry.TileIndex} and Corner {entry.Corner}");
        }

        // Verify Adjacency Lookup
        foreach (var (from, to, direction) in frozenAdjacencyLookup.Adjacencies)
        {
            var deserializedDirection = deserializedTileAtlas.AdjacencyLookup.Get(from, to);
            Assert.That(() => deserializedDirection == direction,
                $"Adjacency mismatch between Tile {from.Index} and Tile {to.Index}");
        }

        // Verify Neighbors
        foreach (var tile in gridConfiguration.GetTileIndices())
        {
            var originalNeighbors = frozenAdjacencyLookup
                .GetNeighbors(tile)
                .ToList();
            var deserializedNeighbors = deserializedTileAtlas
                .AdjacencyLookup.GetNeighbors(tile)
                .ToList();

            Assert.That(() => originalNeighbors.SequenceEqual(deserializedNeighbors),
                $"Neighbors mismatch for Tile {tile.Index}");
        }

        // Verify Weight Lookup
        foreach (var (tileIndex, weight) in frozenWeightLookup.Weights)
        {
            var deserializedWeight = deserializedTileAtlas.WeightLookup.GetWeight(tileIndex);
            Assert.That(() => Math.Abs(deserializedWeight - weight) < 0.001f,
                $"Weight mismatch for Tile {tileIndex.Index}");
        }

        Console.WriteLine("Serialization and deserialization successful!");
    }
}