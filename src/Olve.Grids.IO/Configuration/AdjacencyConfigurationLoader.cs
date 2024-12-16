using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class AdjacencyConfigurationLoader
{
    private readonly AdjacencyConfigurationParser _adjacencyConfigurationParser = new();

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
        if (
            !_adjacencyConfigurationParser
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
            adjacencyLookupBuilder.Clear(adjacency.Tile, adjacency.AdjacencyDirectionToOverwrite);
        }
    }

    private void SetConfiguredAdjacencies(
        AdjacencyConfiguration adjacencyConfiguration,
        IAdjacencyLookupBuilder adjacencyLookupBuilder
    )
    {
        foreach (var adjacency in adjacencyConfiguration.Adjacencies)
        {
            foreach (var adjacent in adjacency.Adjacents)
            {
                if (adjacent.IsAdjacent)
                {
                    adjacencyLookupBuilder.Set(adjacency.Tile, adjacent.Tile, adjacent.Direction);
                }
                else
                {
                    adjacencyLookupBuilder.Remove(
                        adjacency.Tile,
                        adjacent.Tile,
                        adjacent.Direction
                    );
                }
            }
        }
    }
}