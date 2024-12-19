using Olve.Grids.Primitives;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

public class AdjacencyDirectionParser
{
    private static readonly Dictionary<string, Direction> DirectionLookup = new()
    {
        ["up"] = Direction.Up,
        ["u"] = Direction.Up,
        ["down"] = Direction.Down,
        ["d"] = Direction.Down,
        ["left"] = Direction.Left,
        ["l"] = Direction.Left,
        ["right"] = Direction.Right,
        ["r"] = Direction.Right,
    };

    public OneOf<Direction, FileParsingError> ParseAdjacencyDirection(
        string? direction,
        bool required
    )
    {
        if (direction is null)
        {
            if (required)
            {
                return FileParsingError.New("Direction is required.");
            }

            return Direction.None;
        }

        var components = direction
            .Split('|')
            .Select(x => x.Trim())
            .Select(ParseAdjacencyDirectionInternal)
            .ToArray();

        if (components.AnyT1())
        {
            var errors = components.OfT1();
            return FileParsingError.Combine(errors);
        }

        return components
            .OfT0()
            .Aggregate((a, b) => a | b);
    }

    private OneOf<Direction, FileParsingError> ParseAdjacencyDirectionInternal(
        string? direction
    )
    {
        if (direction is null)
        {
            return FileParsingError.New("Direction is required.");
        }

        if (!DirectionLookup.TryGetValue(direction, out var adjacencyDirection))
        {
            return FileParsingError.New(
                "Invalid direction {0}. Allowed values are: {1}",
                direction,
                string.Join(", ", DirectionLookup.Keys)
            );
        }

        return adjacencyDirection;
    }
}