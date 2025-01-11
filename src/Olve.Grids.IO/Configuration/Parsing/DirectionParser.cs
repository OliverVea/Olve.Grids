using Olve.Grids.Primitives;

namespace Olve.Grids.IO.Configuration.Parsing;

public class DirectionParser
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

    public Result<Direction> ParseDirection(
        string? direction,
        bool required
    )
    {
        if (direction is null)
        {
            if (required)
            {
                return new ResultProblem("Direction is required.");
            }

            return Direction.None;
        }

        var directionResults = direction
            .Split('|')
            .Select(x => x.Trim())
            .Select(ParseAdjacencyDirectionInternal);

        if (directionResults.TryPickProblems(out var problems, out var directions))
        {
            return problems;
        }

        return directions.Aggregate((a, b) => a | b);
    }

    private Result<Direction> ParseAdjacencyDirectionInternal(string? direction)
    {
        if (direction is null)
        {
            return new ResultProblem("Direction is required");
        }

        if (!DirectionLookup.TryGetValue(direction, out var adjacencyDirection))
        {
            return new ResultProblem(
                "Invalid direction {0}. Allowed values are: {1}",
                direction,
                string.Join(", ", DirectionLookup.Keys)
            );
        }

        return adjacencyDirection;
    }
}