using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration;
using Olve.Grids.IO.Readers;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Grids.Weights;
using Spectre.Console;

namespace Demo;

public class TileAtlasLoader
{
    public Result<TileAtlas> LoadTileAtlas(
        Size imageSize,
        Size tileSize,
        string tileAtlasBrushesFile,
        string? tileAtlasConfigFile)
    {
        var tileAtlasBuilder = new TileAtlasBuilder()
            .WithTileSize(tileSize);

        var tileAtlasBrushesResult = new TileAtlasBrushesFileReader(tileAtlasBrushesFile).Load();
        if (tileAtlasBrushesResult.TryPickProblems(out var problems, out var brushes))
        {
            return problems;
        }

        var tileIndices = brushes
            .TileBrushes
            .Select(x => x.TileIndex)
            .Distinct()
            .ToArray();

        var adjacencyAndWeightLookupResult =
            LoadAdjacencyLookupAndWeightLookup(tileAtlasConfigFile, tileIndices, brushes.TileBrushes);
        if (adjacencyAndWeightLookupResult.TryPickProblems(out problems, out var adjacencyAndWeightLookup))
        {
            return problems;
        }

        var (adjacencyLookup, weightLookup) = adjacencyAndWeightLookup;

        tileAtlasBuilder = tileAtlasBuilder
            .WithImageSize(imageSize)
            .WithBrushLookup(brushes)
            .WithAdjacencyLookup(adjacencyLookup);

        if (weightLookup is { })
        {
            tileAtlasBuilder = tileAtlasBuilder.WithWeightLookup(weightLookup);
        }

        try
        {
            var tileAtlas = tileAtlasBuilder.Build();
            return tileAtlas;
        }
        catch (InvalidOperationException ex)
        {
            return new ResultProblem(ex, "Could not build tile atlas");
        }
    }

    private Result<(AdjacencyLookup, WeightLookup?)> LoadAdjacencyLookupAndWeightLookup(
        string? tileAtlasConfigFile,
        IEnumerable<TileIndex> tileIndices,
        TileBrushes tileAtlasBrushes)
    {
        var adjacencyLookup = new AdjacencyLookup();

        if (tileAtlasConfigFile is null)
        {
            AnsiConsole.MarkupLine(
                "[yellow]No tile atlas configuration file provided, using default adjacency estimator.[/]");

            EstimateAdjacenciesFromBrushesCommand.Request request = new(adjacencyLookup, tileAtlasBrushes);
            new EstimateAdjacenciesFromBrushesCommand().Execute(request);
            return (adjacencyLookup, null);
        }

        var weightLookup = new WeightLookup();

        var configurationLoader = ConfigurationLoader.Create();
        var configurationResult = configurationLoader.Load(tileAtlasConfigFile,
            adjacencyLookup,
            weightLookup,
            tileIndices,
            tileAtlasBrushes);
        if (configurationResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        return (adjacencyLookup, weightLookup);
    }
}