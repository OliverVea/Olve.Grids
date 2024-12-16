using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfigurationLoader(AdjacencyConfigurationParser adjacencyConfigurationParser)
{
    public OneOf<IAdjacencyLookupBuilder, FileParsingError> LoadAdjacencyLookupBuilder(
        ConfigurationModel configurationModel,
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushConfiguration
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
        IAdjacencyLookupBuilder adjacencyLookupBuilder,
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushConfiguration
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
            GenerateFromBrushes(adjacencyLookupBuilder, brushConfiguration);
        }

        ClearAdjacenciesToOverwrite(adjacencyConfiguration, adjacencyLookupBuilder);
        SetConfiguredAdjacencies(adjacencyConfiguration, adjacencyLookupBuilder);

        return new Success();
    }

    private void GenerateFromBrushes(
        IAdjacencyLookupBuilder adjacencyLookupBuilder,
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushConfiguration
    )
    {
        var b = new AdjacencyFromTileBrushEstimator();
        b.SetAdjacencies(adjacencyLookupBuilder, brushConfiguration);
    }

    private void ClearAdjacenciesToOverwrite(
        AdjacencyConfiguration adjacencyConfiguration,
        IAdjacencyLookupBuilder adjacencyLookupBuilder
    )
    {
        foreach (var adjacency in adjacencyConfiguration.Adjacencies)
        {
            foreach (var tile in adjacency.Tiles)
            {
                adjacencyLookupBuilder.Clear(tile, adjacency.AdjacencyDirectionToOverwrite);
            }
        }
    }

    private void SetConfiguredAdjacencies(
        AdjacencyConfiguration adjacencyConfiguration,
        IAdjacencyLookupBuilder adjacencyLookupBuilder
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
                            adjacencyLookupBuilder.Set(adjacencyTile, adjacentTile, adjacent.Direction);
                        }
                        else
                        {
                            adjacencyLookupBuilder.Remove(
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