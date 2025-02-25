﻿using MemoryPack;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Serialization.Models;
using Olve.Grids.Weights;

namespace Olve.Grids.IO.TileAtlasBuilder;

[MemoryPackable]
public partial class SerializableTileAtlasConfiguration
{
    public Size? TileSize { get; init; }
    public Size? ImageSize { get; init; }
    public int? Columns { get; init; }
    public int? Rows { get; init; }
    public TileIndex? FallbackTileIndex { get; init; }

    public SerializableAdjacencyLookup? AdjacencyLookup { get; init; }
    public SerializableBrushLookup? BrushLookup { get; init; }
    public SerializableWeightLookup? WeightLookup { get; init; }

    public static SerializableTileAtlasConfiguration FromTileAtlasConfiguration(
        TileAtlasConfiguration tileAtlasConfiguration) =>
        new()
        {
            TileSize = tileAtlasConfiguration.TileSize,
            ImageSize = tileAtlasConfiguration.ImageSize,
            Columns = tileAtlasConfiguration.Columns,
            Rows = tileAtlasConfiguration.Rows,
            FallbackTileIndex = tileAtlasConfiguration.FallbackTileIndex,
            AdjacencyLookup =
                SerializableAdjacencyLookup.FromAdjacencyLookup(tileAtlasConfiguration.AdjacencyLookup
                                                                ?? new AdjacencyLookup()),
            BrushLookup = SerializableBrushLookup.FromBrushLookup(tileAtlasConfiguration.BrushLookup ?? new BrushLookup()),
            WeightLookup =
                SerializableWeightLookup.FromWeightLookup(tileAtlasConfiguration.WeightLookup ?? new WeightLookup()),
        };

    public TileAtlasConfiguration ToTileAtlasConfiguration() =>
        new()
        {
            TileSize = TileSize,
            ImageSize = ImageSize,
            Columns = Columns,
            Rows = Rows,
            FallbackTileIndex = FallbackTileIndex,
            AdjacencyLookup =
                AdjacencyLookup is null
                    ? new AdjacencyLookup()
                    : AdjacencyLookup.ToAdjacencyLookup(),
            BrushLookup = BrushLookup is null
                ? new BrushLookup()
                : BrushLookup.ToBrushLookup(),
            WeightLookup = WeightLookup is null
                ? new WeightLookup()
                : WeightLookup.ToWeightLookup(),
        };
}