using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Primitives;


namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfigurationLoader(AdjacencyConfigurationParser adjacencyConfigurationParser)
{
    public Result<IAdjacencyLookup> LoadAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
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
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        var adjacencyConfigurationResult = adjacencyConfigurationParser.Parse(configurationModel);
        if (adjacencyConfigurationResult.TryPickProblems(out var problems, out var adjacencyConfiguration))
        {
            return problems;
        }

        if (adjacencyConfiguration.GenerateFromBrushes)
        {
            GenerateFromBrushes(adjacencyLookup, brushConfiguration);
        }

        ClearAdjacenciesToOverwrite(adjacencyConfiguration, adjacencyLookup);
        SetConfiguredAdjacencies(adjacencyConfiguration, adjacencyLookup);

        return Result.Success();
    }

    private static void GenerateFromBrushes(
        IAdjacencyLookup adjacencyLookup,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        var b = new AdjacencyFromTileBrushEstimator();
        b.SetAdjacencies(adjacencyLookup, brushConfiguration);
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
                        if (adjacent.IsAdjacent)
                        {
                            adjacencyLookup.Set(adjacencyTile, adjacentTile, adjacent.Direction);
                        }
                        else
                        {
                            adjacencyLookup.Remove(
                                adjacencyTile,
                                adjacentTile,
                                adjacent.Direction
                            );
                        }
                    }
                }
            }
        }
    }
}