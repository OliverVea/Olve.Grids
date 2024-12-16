using Olve.Grids.Adjacencies;
using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

internal class AdjacencyConfigurationParser : IParser<AdjacencyConfiguration>
{
    private readonly AdjacencyDirectionParser _adjacencyDirectionParser = new();
    private readonly TileIndexParser _tileIndexParser = new();

    public OneOf<AdjacencyConfiguration, FileParsingError> Parse(
        ConfigurationModel configurationModel
    )
    {
        if (
            !ParseAdjacencies(configurationModel)
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

    private OneOf<
        IReadOnlyList<AdjacencyConfiguration.Adjacency>,
        FileParsingError
    > ParseAdjacencies(ConfigurationModel configurationModel)
    {
        if (configurationModel.Adjacencies is not { } adjacencyModels)
        {
            return Array.Empty<AdjacencyConfiguration.Adjacency>();
        }

        var adjacencyParsingResults = adjacencyModels
            .Select(ParseAdjacency)
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
        AdjacencyModel adjacencyModel
    )
    {
        if (
            !_tileIndexParser
                .Parse(adjacencyModel.Tile)
                .TryPickT0(out var tileIndex, out var tileError)
        )
        {
            return tileError;
        }

        var adjacents = adjacencyModel.Adjacents is { } adjacentModels
            ? adjacentModels
                .Select(ParseAdjacent)
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
                .Select(x => _adjacencyDirectionParser.ParseAdjacencyDirection(x, false))
                .ToArray()
            : [ ];

        if (!overwriteDirections.AllT0())
        {
            var errors = overwriteDirections.OfT1();
            return FileParsingError.Combine(errors);
        }

        var adjacencyDirection = overwriteDirections
            .OfT0()
            .Aggregate(AdjacencyDirection.None, (acc, x) => acc | x);

        return new AdjacencyConfiguration.Adjacency
        {
            Tile = tileIndex,
            AdjacencyDirectionToOverwrite = adjacencyDirection,
            Adjacents = adjacents
                .OfT0()
                .ToArray(),
        };
    }

    private OneOf<AdjacencyConfiguration.Adjacent, FileParsingError> ParseAdjacent(
        AdjacentModel adjacentModel
    )
    {
        if (
            !_tileIndexParser
                .Parse(adjacentModel.Tile)
                .TryPickT0(out var tileIndex, out var tileIndexError)
        )
        {
            return tileIndexError;
        }

        if (
            !_adjacencyDirectionParser
                .ParseAdjacencyDirection(adjacentModel.Direction, true)
                .TryPickT0(out var direction, out var directionError)
        )
        {
            return directionError;
        }

        return new AdjacencyConfiguration.Adjacent
        {
            Tile = tileIndex,
            IsAdjacent = adjacentModel.IsAdjacent,
            Direction = direction,
        };
    }
}