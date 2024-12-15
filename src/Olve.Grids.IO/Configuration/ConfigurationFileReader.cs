using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration;

public class ConfigurationFileReader(string filePath)
{
    private readonly ConfigurationModelFileReader _configurationModelFileReader = new(filePath);
    
    public OneOf<AdjacencyConfiguration, FileParsingError> ReadAdjacencyConfiguration()
    {
        if (!_configurationModelFileReader.Read().TryPickT0(
                out var configurationModel,
                out var configurationError))
        {
            return configurationError;
        }

        if (!ParseAdjacencies(configurationModel).TryPickT0(
            out var adjacencies,
            out var adjacenciesError))
        {
            return adjacenciesError;
        }

        return new AdjacencyConfiguration
        {
            GenerateFromBrushes = configurationModel.GenerateAdjacenciesFromBrushes,
            Adjacencies = adjacencies
        };
    }
    
    private OneOf<IReadOnlyList<AdjacencyConfiguration.Adjacency>, FileParsingError> ParseAdjacencies(ConfigurationModel configurationModel)
    {
        if (configurationModel.Adjacencies is not { } adjacencyModels)
        {
            return Array.Empty<AdjacencyConfiguration.Adjacency>();
        }
        
        var adjacencyParsingResults = adjacencyModels.Select(ParseAdjacency).ToArray();

        if (adjacencyParsingResults.HasT1())
        {
            var errors = adjacencyParsingResults.OfT1();
            return FileParsingError.Combine(errors);
        }
        
        return adjacencyParsingResults.OfT0().ToArray();
    }
    
    private OneOf<AdjacencyConfiguration.Adjacency, FileParsingError> ParseAdjacency(AdjacencyModel adjacencyModel)
    {
        if (!ParseTileIndex(adjacencyModel.Tile).TryPickT0(
            out var tileIndex,
            out var tileError))
        {
            return tileError;
        }
        
        OneOf<AdjacencyConfiguration.Adjacent, FileParsingError>[] adjacents = adjacencyModel.Adjacents is { } adjacentModels ? 
            adjacentModels.Select(ParseAdjacent).ToArray() : 
            [];

        if (adjacents.HasT1())
        {
            var errors = adjacents.OfT1();
            return FileParsingError.Combine(errors);
        }

        return new AdjacencyConfiguration.Adjacency
        {
            Tile = tileIndex,
            Adjacents = adjacents.OfT0().ToArray()
        };
    }

    private OneOf<AdjacencyConfiguration.Adjacent, FileParsingError> ParseAdjacent(AdjacentModel adjacentModel)
    {
        if (!ParseTileIndex(adjacentModel.Tile).TryPickT0(
            out var tileIndex,
            out var tileIndexError))
        {
            return tileIndexError;
        }
        
        if (!ParseAdjacencyDirection(adjacentModel.Direction).TryPickT0(
            out var direction,
            out var directionError))
        {
            return directionError;
        }


        return new AdjacencyConfiguration.Adjacent
        {
            Tile = tileIndex,
            IsAdjacent = adjacentModel.IsAdjacent,
            Direction = 
        };
    }
    
    private OneOf<TileIndex, FileParsingError> ParseTileIndex(int? tile)
    {
        if (tile is not {} tileIndex)
        {
            return FileParsingError.New("Tile is required.");
        }
        
        if (tileIndex < 0)
        {
            return FileParsingError.New("Tile must be non-negative.");
        }

        return new TileIndex(tileIndex);
    }
    
    private static readonly Dictionary<string, AdjacencyDirection> DirectionLookup = new()
    {
        ["up"] = AdjacencyDirection.Up,
        ["u"] = AdjacencyDirection.Up,
        ["down"] = AdjacencyDirection.Down,
        ["d"] = AdjacencyDirection.Down,
        ["left"] = AdjacencyDirection.Left,
        ["l"] = AdjacencyDirection.Left,
        ["right"] = AdjacencyDirection.Right,
        ["r"] = AdjacencyDirection.Right
    };
    
    private OneOf<AdjacencyDirection, FileParsingError> ParseAdjacencyDirection(string? direction)
    {
        if (direction is not {} directionValue)
        {
            return FileParsingError.New("Direction is required.");
        }
        
        if (!DirectionLookup.TryGetValue(directionValue, out var adjacencyDirection))
        {
            return FileParsingError.New("Invalid direction ø{0}. Allowed values are: ");
        }

        return adjacencyDirection;
    }
    
    
}