using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;


namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfigurationLoader(AdjacencyConfigurationParser adjacencyConfigurationParser)
{
    public Result<IAdjacencyLookup> LoadAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        TileBrushes brushConfiguration)
    {
        var adjacencyLookupBuilder = new AdjacencyLookup();

        var result = ConfigureAdjacencyLookupBuilder(
            configurationModel,
            adjacencyLookupBuilder,
            brushConfiguration
        );

        if (result.TryPickProblems(out var problems))
        {
            return problems;
        }

        return adjacencyLookupBuilder;
    }

    public Result ConfigureAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        IAdjacencyLookup adjacencyLookup,
        TileBrushes brushConfiguration
    )
    {
        var adjacencyConfigurationResult = adjacencyConfigurationParser.Parse(configurationModel);
        if (adjacencyConfigurationResult.TryPickProblems(out var problems, out var adjacencyConfiguration))
        {
            return problems;
        }

        if (adjacencyConfiguration.GenerateFromBrushes)
        {
            EstimateAdjacenciesFromBrushesOperation.Request request = new(adjacencyLookup, brushConfiguration);
            new EstimateAdjacenciesFromBrushesOperation().Execute(request);
        }

        ClearAdjacenciesToOverwrite(adjacencyConfiguration, adjacencyLookup);
        SetConfiguredAdjacencies(adjacencyConfiguration, adjacencyLookup);

        return Result.Success();
    }

    private static void ClearAdjacenciesToOverwrite(
        AdjacencyConfiguration adjacencyConfiguration,
        IAdjacencyLookup adjacencyLookup
    )
    {
        foreach (var adjacency in adjacencyConfiguration.Adjacencies)
        {
            foreach (var tile in adjacency.Tiles)
            {
                adjacencyLookup.Clear(tile, adjacency.DirectionToOverwrite);
            }
        }
    }

    private static void SetConfiguredAdjacencies(
        AdjacencyConfiguration adjacencyConfiguration,
        IAdjacencyLookup adjacencyLookup
    )
    {
        foreach (var adjacency in adjacencyConfiguration.Adjacencies)
        {
            foreach (var adjacencyTile in adjacency.Tiles)
            {
                foreach (var adjacent in adjacency.Adjacents)
                {
                    foreach (var adjacentTile in adjacent.Tiles)
                    {
                        var tileAdjacency = new TileAdjacency(
                            adjacencyTile,
                            adjacentTile,
                            adjacent.Direction
                        );

                        adjacencyLookup.Set(tileAdjacency, adjacent.IsAdjacent);
                    }
                }
            }
        }
    }
}