using Olve.Grids.Adjacencies;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.Primitives;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

public class AdjacencyConfigurationParser(
    TileGroupParser tileGroupParser,
    AdjacencyDirectionParser adjacencyDirectionParser)
    : IParser<AdjacencyConfiguration>
{
    public OneOf<AdjacencyConfiguration, FileParsingError> Parse(
        ConfigurationModel configurationModel
    )
    {
        if (!tileGroupParser
                .Parse(configurationModel)
                .TryPickT0(out var tileGroups, out var tileGroupsError))
        {
            return tileGroupsError;
        }

        if (
            !ParseAdjacencies(tileGroups, configurationModel)
                .TryPickT0(out var adjacencies, out var adjacenciesError)
        )
        {
            return adjacenciesError;
        }

        return new AdjacencyConfiguration
        {
            GenerateFromBrushes = configurationModel.GenerateAdjacenciesFromBrushes,
            Adjacencies = adjacencies,
        };
    }

    private OneOf<IReadOnlyList<AdjacencyConfiguration.Adjacency>, FileParsingError> ParseAdjacencies(
        TileGroups tileGroups,
        ConfigurationModel configurationModel)
    {
        if (configurationModel.Adjacencies is not { } adjacencyModels)
        {
            return Array.Empty<AdjacencyConfiguration.Adjacency>();
        }

        var adjacencyParsingResults = adjacencyModels
            .Select(x => ParseAdjacency(tileGroups, x))
            .ToArray();

        if (!adjacencyParsingResults.AllT0())
        {
            var errors = adjacencyParsingResults.OfT1();
            return FileParsingError.Combine(errors);
        }

        return adjacencyParsingResults
            .OfT0()
            .ToArray();
    }

    private OneOf<AdjacencyConfiguration.Adjacency, FileParsingError> ParseAdjacency(
        TileGroups tileGroups,
        AdjacencyModel adjacencyModel
    )
    {
        if (!tileGroupParser
                .Parse(adjacencyModel.Tiles, adjacencyModel.Group, tileGroups)
                .TryPickT0(out var tiles, out var tileError)
           )
        {
            return tileError;
        }

        var adjacents = adjacencyModel.Adjacents is { } adjacentModels
            ? adjacentModels
                .Select(x => ParseAdjacent(tileGroups, x))
                .ToArray()
            : [ ];

        if (!adjacents.AllT0())
        {
            var errors = adjacents.OfT1();
            return FileParsingError.Combine(errors);
        }

        var overwriteDirections = adjacencyModel.OverwriteBrushAdjacencies
            is { } overwriteDirectionsModel
            ? overwriteDirectionsModel
                .Select(x => adjacencyDirectionParser.ParseAdjacencyDirection(x, false))
                .ToArray()
            : [ ];

        if (!overwriteDirections.AllT0())
        {
            var errors = overwriteDirections.OfT1();
            return FileParsingError.Combine(errors);
        }

        var adjacencyDirection = overwriteDirections
            .OfT0()
            .Aggregate(Direction.None, (acc, x) => acc | x);

        return new AdjacencyConfiguration.Adjacency
        {
            Tiles = tiles.ToArray(),
            DirectionToOverwrite = adjacencyDirection,
            Adjacents = adjacents
                .OfT0()
                .ToArray(),
        };
    }

    private OneOf<AdjacencyConfiguration.Adjacent, FileParsingError> ParseAdjacent(
        TileGroups tileGroups,
        AdjacentModel adjacentModel
    )
    {
        if (!tileGroupParser
                .Parse(adjacentModel.Tiles, adjacentModel.Group, tileGroups)
                .TryPickT0(out var tiles, out var tileIndexError)
           )
        {
            return tileIndexError;
        }

        if (
            !adjacencyDirectionParser
                .ParseAdjacencyDirection(adjacentModel.Direction, true)
                .TryPickT0(out var direction, out var directionError)
        )
        {
            return directionError;
        }

        return new AdjacencyConfiguration.Adjacent
        {
            Tiles = tiles.ToArray(),
            IsAdjacent = adjacentModel.IsAdjacent,
            Direction = direction,
        };
    }
}