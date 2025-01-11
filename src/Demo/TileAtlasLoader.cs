using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration;
using Olve.Grids.IO.Readers;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Grids.Primitives;
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
        if (tileAtlasBrushesResult.TryPickProblems(out var problems, out var brushLookup))
        {
            return problems;
        }

        var tileIndices = brushLookup
            .Entries
            .Select(x => x.TileIndex)
            .Distinct()
            .ToArray();

        var adjacencyAndWeightLookupResult = LoadAdjacencyLookupAndWeightLookup(tileAtlasConfigFile, tileIndices, brushLookup.Entries);
        if (adjacencyAndWeightLookupResult.TryPickProblems(out problems, out var adjacencyAndWeightLookup))
        {
            return problems;
        }

        var (adjacencyLookup, weightLookup) = adjacencyAndWeightLookup;

        tileAtlasBuilder = tileAtlasBuilder
            .WithImageSize(imageSize)
            .WithBrushLookup(brushLookup)
            .WithAdjacencyLookup(adjacencyLookup);

        if (weightLookup is { })
        {
            tileAtlasBuilder = tileAtlasBuilder.WithWeightLookup(weightLookup);
        }

        try
        {
            return tileAtlasBuilder.Build();
        }
        catch (InvalidOperationException ex)
        {
            return new ResultProblem(ex, "Could not build tile atlas");
        }
    }

    private Result<(AdjacencyLookup, WeightLookup?)> LoadAdjacencyLookupAndWeightLookup(
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