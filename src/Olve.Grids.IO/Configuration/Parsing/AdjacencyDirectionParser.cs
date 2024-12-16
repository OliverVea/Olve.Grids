using Olve.Grids.Adjacencies;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

internal class AdjacencyDirectionParser
{
    private static readonly Dictionary<string, AdjacencyDirection> DirectionLookup = new()
    {
        ["up"] = AdjacencyDirection.Up,
        ["u"] = AdjacencyDirection.Up,
        ["down"] = AdjacencyDirection.Down,
        ["d"] = AdjacencyDirection.Down,
        ["left"] = AdjacencyDirection.Left,
        ["l"] = AdjacencyDirection.Left,
        ["right"] = AdjacencyDirection.Right,
        ["r"] = AdjacencyDirection.Right,
    };

    public OneOf<AdjacencyDirection, FileParsingError> ParseAdjacencyDirection(
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

            return AdjacencyDirection.None;
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

        return components.OfT0().Aggregate((a, b) => a | b);
    }

    private OneOf<AdjacencyDirection, FileParsingError> ParseAdjacencyDirectionInternal(
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