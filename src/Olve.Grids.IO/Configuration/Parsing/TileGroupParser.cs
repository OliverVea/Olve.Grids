using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

public class TileGroupParser(TileIndexParser tileIndexParser)
{
    private static readonly TileGroups EmptyTileGroups = new();

    public OneOf<TileGroups, FileParsingError> Parse(ConfigurationModel configurationModel)
    {
        if (configurationModel.Groups is not { } groups)
        {
            return EmptyTileGroups;
        }

        var groupResults = groups
            .Select(x => ParseGroup(x.Key, x.Value))
            .ToArray();

        if (groupResults.AnyT1())
        {
            var errors = groupResults.OfT1();
            return FileParsingError.Combine(errors);
        }

        var tileGroups = groupResults
            .OfT0()
            .ToDictionary(x => x.GroupName, x => x.Tiles);

        return new TileGroups
        {
            Groups = tileGroups,
        };
    }

    private OneOf<(string GroupName, IEnumerable<TileIndex> Tiles), FileParsingError> ParseGroup(
        string groupName,
        GroupModel groupModel)
    {
        if (groupModel.Tiles is not { } tiles)
        {
            return FileParsingError.New("Tiles are required.");
        }

        return tileIndexParser
            .Parse(tiles)
            .Match<OneOf<(string GroupName, IEnumerable<TileIndex> Tiles), FileParsingError>>(
                x => (groupName, x),
                x => x);
    }

    public OneOf<IEnumerable<TileIndex>, FileParsingError> Parse(string? tiles, string? group, TileGroups tileGroups) =>
        tiles is null
            ? group is null
                ? FileParsingError.New("Tiles or Group must be specified")
                : ParseGroup(group, tileGroups)
            : group is null
                ? ParseTiles(tiles)
                : FileParsingError.New("Tiles and Group cannot be specified at the same time");

    private OneOf<IEnumerable<TileIndex>, FileParsingError> ParseTiles(string tiles) => tileIndexParser.Parse(tiles);

    private OneOf<IEnumerable<TileIndex>, FileParsingError> ParseGroup(string group, TileGroups tileGroups)
    {
        if (!tileGroups.Groups.TryGetValue(group, out var tileGroup))
        {
            return FileParsingError.New($"Group '{group}' not found");
        }

        return OneOf<IEnumerable<TileIndex>, FileParsingError>.FromT0(tileGroup);
    }
}