using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;

namespace Olve.Grids.IO.Configuration.Parsing;

public class TileGroupParser(TileIndexParser tileIndexParser)
{
    private static readonly TileGroups EmptyTileGroups = new();

    public Result<TileGroups> Parse(ConfigurationModel configurationModel)
    {
        if (configurationModel.Groups is not { } groups)
        {
            return EmptyTileGroups;
        }

        var groupResults = groups.Select(x => ParseGroup(x.Key, x.Value));

        if (groupResults.TryPickProblems(out var problems, out var tileGroups))
        {
            return problems;
        }

        return new TileGroups
        {
            Groups = tileGroups.ToDictionary(x => x.GroupName, x => x.Tiles)
        };
    }

    private Result<(string GroupName, IEnumerable<TileIndex> Tiles)> ParseGroup(
        string groupName,
        GroupModel groupModel)
    {
        if (groupModel.Tiles is not { } tileString)
        {
            var problem = new ResultProblem("Tiles are required");
            return Result<(string, IEnumerable<TileIndex>)>.Failure(problem);
        }

        var parsingResult = tileIndexParser.Parse(tileString);
        if (parsingResult.TryPickProblems(out var problems, out var tiles))
        {
            return problems;
        }
        
        return Result<(string GroupName, IEnumerable<TileIndex> Tiles)>.Success((groupName, tiles));
    }

    public Result<IEnumerable<TileIndex>> Parse(string? tiles, string? group, TileGroups tileGroups)
    {
        if (tiles is { } && group is { })
        {
            return new ResultProblem("Tiles and Group cannot be specified at the same time");
        }

        if (group is { })
        {
            return ParseGroup(group, tileGroups);
        }

        if (tiles is { })
        {
            return ParseTiles(tiles);
        }
        
        return new ResultProblem("Tiles or Group must be specified");

    }

    private Result<IEnumerable<TileIndex>> ParseTiles(string tiles) => tileIndexParser.Parse(tiles);

    private Result<IEnumerable<TileIndex>> ParseGroup(string group, TileGroups tileGroups)
    {
        if (!tileGroups.Groups.TryGetValue(group, out var tileGroup))
        {
            return new ResultProblem("Group must be specified");
        }

        return Result<IEnumerable<TileIndex>>.Success(tileGroup);
    }
}