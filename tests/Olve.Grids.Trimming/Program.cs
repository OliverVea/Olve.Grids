// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Serialization;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Grids.Serialization;
using Olve.Grids.Serialization.Models;
using Olve.Grids.Weights;
using Olve.Utilities.Assertions;


GridConfiguration gridConfiguration = new((16, 16), 16, 16);
FrozenBrushLookup frozenBrushLookup = new([ (new TileIndex(13), Corner.UpperLeft, new BrushId("brush")), ]);
FrozenAdjacencyLookup frozenAdjacencyLookup =
    new([ (new TileIndex(13), new TileIndex(14), Direction.Left | Direction.Down), ]);
FrozenWeightLookup frozenWeightLookup =
    new([
            new KeyValuePair<TileIndex, float>(new TileIndex(13), 10),
            new KeyValuePair<TileIndex, float>(new TileIndex(14), 5),
        ],
        42);

TileAtlas tileAtlas = new(gridConfiguration, frozenBrushLookup, frozenAdjacencyLookup, frozenWeightLookup);

TileAtlasSerializer serializer = new();

var bytes = serializer.Serialize(tileAtlas);

var deserialized = serializer.Deserialize(bytes);

Assert.NotNull(deserialized);

var serializableDeserialized = SerializableTileAtlas.FromTileAtlas(deserialized);

var json = JsonSerializer.Serialize(serializableDeserialized, TileAtlasContext.Default.SerializableTileAtlas);

Console.WriteLine(json);

[JsonSerializable(typeof(SerializableTileAtlas))]
public partial class TileAtlasContext : JsonSerializerContext;