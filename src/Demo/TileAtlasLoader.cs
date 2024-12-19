using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.IO;
using Olve.Grids.IO.Configuration;
using Olve.Grids.IO.Readers;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Grids.Primitives;
using Olve.Grids.Weights;
using Spectre.Console;

namespace Demo;

public class TileAtlasLoader
{
    public OneOf<TileAtlas, FileParsingError> LoadTileAtlas(
        string tileAtlasFile,
        Size tileSize,
        string tileAtlasBrushesFile,
        string? tileAtlasConfigFile)
    {
        var tileAtlasBuilder = new TileAtlasBuilder()
            .WithFilePath(tileAtlasFile)
            .WithTileSize(tileSize);

        var tileAtlasBrushesReader = new TileAtlasBrushesFileReader(tileAtlasBrushesFile);
        if (!tileAtlasBrushesReader
                .Load()
                .TryPickT0(out var tileAtlasBrushes, out var brushErrors))
        {
            return brushErrors;
        }

        var tileIndices = tileAtlasBrushes
            .Entries
            .Select(x => x.TileIndex)
            .Distinct()
            .ToArray();

        if (!LoadAdjacencyLookupAndWeightLookup(tileAtlasConfigFile, tileIndices, tileAtlasBrushes.Entries)
                .TryPickT0(out var adjacencyAndWeightLookup, out var atlasConfigurationError))
        {
            return atlasConfigurationError;
        }

        var (adjacencyLookup, weightLookup) = adjacencyAndWeightLookup;

        tileAtlasBuilder = tileAtlasBuilder
            .WithBrushLookupBuilder(tileAtlasBrushes)
            .WithAdjacencyLookupBuilder(adjacencyLookup);

        if (weightLookup is { })
        {
            tileAtlasBuilder = tileAtlasBuilder.WithWeightLookupBuilder(weightLookup);
        }

        return tileAtlasBuilder.Build();
    }

    private OneOf<(AdjacencyLookup, WeightLookup?), FileParsingError> LoadAdjacencyLookupAndWeightLookup(
        string? tileAtlasConfigFile,
        IEnumerable<TileIndex> tileIndices,
        IEnumerable<(TileIndex, Corner, BrushId)> tileAtlasBrushes)
    {
        var adjacencyLookup = new AdjacencyLookup();

        if (tileAtlasConfigFile is null)
        {
            AnsiConsole.MarkupLine(
                "[yellow]No tile atlas configuration file provided, using default adjacency estimator.[/]");

            var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
            adjacencyEstimator.SetAdjacencies(adjacencyLookup, tileAtlasBrushes);
            return (adjacencyLookup, null);
        }

        var weightLookup = new WeightLookup();

        var configurationLoader = ConfigurationLoader.Create();
        configurationLoader.Load(tileAtlasConfigFile, adjacencyLookup, weightLookup, tileIndices, tileAtlasBrushes);

        return (adjacencyLookup, weightLookup);
    }
}