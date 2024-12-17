using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Primitives;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfigurationLoader(AdjacencyConfigurationParser adjacencyConfigurationParser)
{
    public OneOf<IAdjacencyLookup, FileParsingError> LoadAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        var adjacencyLookupBuilder = new AdjacencyLookup();

        if (
            !ConfigureAdjacencyLookupBuilder(
                    configurationModel,
                    adjacencyLookupBuilder,
                    brushConfiguration
                )
                .TryPickT0(out _, out var error)
        )
        {
            return error;
        }

        return adjacencyLookupBuilder;
    }

    public OneOf<Success, FileParsingError> ConfigureAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        IAdjacencyLookup adjacencyLookup,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        if (!adjacencyConfigurationParser
                .Parse(configurationModel)
                .TryPickT0(out var adjacencyConfiguration, out var parsingError)
           )
        {
            return parsingError;
        }

        if (adjacencyConfiguration.GenerateFromBrushes)
        {
            GenerateFromBrushes(adjacencyLookup, brushConfiguration);
        }

        ClearAdjacenciesToOverwrite(adjacencyConfiguration, adjacencyLookup);
        SetConfiguredAdjacencies(adjacencyConfiguration, adjacencyLookup);

        return new Success();
    }

    private void GenerateFromBrushes(
        IAdjacencyLookup adjacencyLookup,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration
    )
    {
        var b = new AdjacencyFromTileBrushEstimator();
        b.SetAdjacencies(adjacencyLookup, brushConfiguration);
    }

    private void ClearAdjacenciesToOverwrite(
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

    private void SetConfiguredAdjacencies(
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