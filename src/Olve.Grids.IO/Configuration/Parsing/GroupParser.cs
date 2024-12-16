using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

/*
public class GroupParser(TileIndexParser tileIndexParser) : IParser<TileGroups>
{
    public OneOf<TileGroups, FileParsingError> Parse(ConfigurationModel configurationModel)
    {
        var results = configurationModel
                          .Groups?
                          .Select(x => ParseGroup(x.Key, x.Value))
                          .ToArray()
                      ?? [ ];

        if (results.AnyT1())
        {
            var errors = results.OfT1();
            return FileParsingError.Combine(errors);
        }

        return new TileGroups
        {
            Groups = results
                .OfT0()
                .ToDictionary(x => x.GroupName, x => x.TileIndices),
        };
    }

    private OneOf<(string GroupName, IEnumerable<TileIndex> TileIndices), FileParsingError> ParseGroup(
        string groupName,
        GroupModel groupModel)
    {
        return tileIndexParser
            .Parse(groupModel.Tiles)
            .Match<OneOf<(string GroupName, IEnumerable<TileIndex> TileIndices), FileParsingError>>(x => (groupName, x),
                x => x);
    }
}
*/