using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.Primitives;

namespace Olve.Grids.IO.Configuration.Parsing;

public class AdjacencyConfigurationParser(TileGroupParser tileGroupParser, DirectionParser directionParser) : IParser<AdjacencyConfiguration>
{
    public Result<AdjacencyConfiguration> Parse(ConfigurationModel configurationModel)
    {
        // Todo: Do we need to parse the tile groups again here?
        var tileGroupResults = tileGroupParser.Parse(configurationModel);
        if (tileGroupResults.TryPickProblems(out var problems, out var tileGroups))
        {
            return problems;
        }

        var adjacencyResult = ParseAdjacencies(tileGroups, configurationModel);
        if (adjacencyResult.TryPickProblems(out problems, out var adjacencies))
        {
            return problems;
        }

        return new AdjacencyConfiguration
        {
            GenerateFromBrushes = configurationModel.GenerateAdjacenciesFromBrushes,
            Adjacencies = adjacencies,
        };
    }

    private Result<IReadOnlyList<AdjacencyConfiguration.Adjacency>> ParseAdjacencies(TileGroups tileGroups, ConfigurationModel configurationModel)
    {
        if (configurationModel.Adjacencies is not { } adjacencyModels)
        {
            return Array.Empty<AdjacencyConfiguration.Adjacency>();
        }

        var adjacencyParsingResults = adjacencyModels.Select(x => ParseAdjacency(tileGroups, x));

        if (adjacencyParsingResults.TryPickProblems(out var problems, out var adjacencies))
        {
            return problems;
        }

        return adjacencies.ToArray();
    }

    private Result<AdjacencyConfiguration.Adjacency> ParseAdjacency(TileGroups tileGroups, AdjacencyModel adjacencyModel)
    {
        var tileResult = tileGroupParser.Parse(adjacencyModel.Tiles, adjacencyModel.Group, tileGroups);
        if (tileResult.TryPickProblems(out var problems, out var tileIndices))
        {
            return problems;
        }

        var adjacentResults = adjacencyModel.Adjacents is { } adjacentModels
            ? adjacentModels.Select(x => ParseAdjacent(tileGroups, x))
            : [ ];

        if (!adjacentResults.TryPickProblems(out problems, out var adjacents))
        {
            return problems;
        }

        var overwriteDirectionResults = adjacencyModel.OverwriteBrushAdjacencies
            is { } overwriteDirectionsModel
            ? overwriteDirectionsModel.Select(x => directionParser.ParseDirection(x, false))
            : [ ];

        if (overwriteDirectionResults.TryPickProblems(out problems, out var overwriteDirections))
        {
            return problems;
        }

        var adjacencyDirection = overwriteDirections.Combine();

        return new AdjacencyConfiguration.Adjacency
        {
            Tiles = tileIndices.ToArray(),
            DirectionToOverwrite = adjacencyDirection,
            Adjacents = adjacents.ToArray(),
        };
    }

    private Result<AdjacencyConfiguration.Adjacent> ParseAdjacent(TileGroups tileGroups, AdjacentModel adjacentModel)
    {
        var tileResults = tileGroupParser.Parse(adjacentModel.Tiles, adjacentModel.Group, tileGroups);
        if (tileResults.TryPickProblems(out var problems, out var tileIndices))
        {
            return problems;
        }

        var directionResult = directionParser.ParseDirection(adjacentModel.Direction, required: true);
        if (directionResult.TryPickProblems(out problems, out var adjacencyDirection))
        {
            return problems;
        }

        return new AdjacencyConfiguration.Adjacent
        {
            Tiles = tileIndices.ToArray(),
            IsAdjacent = adjacentModel.IsAdjacent,
            Direction = adjacencyDirection,
        };
    }
}