using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

internal class WeightConfigurationParser : IParser<WeightConfiguration>
{
    private readonly TileIndexParser _tileIndexParser = new();

    public OneOf<WeightConfiguration, FileParsingError> Parse(ConfigurationModel configurationModel)
    {
        if (!ParseWeights(configurationModel).TryPickT0(out var weights, out var error))
        {
            return error;
        }

        return new WeightConfiguration { Weights = weights };
    }

    private OneOf<IReadOnlyList<WeightConfiguration.TileWeight>, FileParsingError> ParseWeights(
        ConfigurationModel configurationModel
    )
    {
        if (configurationModel.Weights is not { } weightModels)
        {
            return Array.Empty<WeightConfiguration.TileWeight>();
        }

        var weightParsingResults = weightModels.Select(ParseWeight).ToArray();

        if (weightParsingResults.AnyT1())
        {
            var errors = weightParsingResults.OfT1();
            return FileParsingError.Combine(errors);
        }

        return weightParsingResults.OfT0().ToArray();
    }

    private OneOf<WeightConfiguration.TileWeight, FileParsingError> ParseWeight(
        WeightModel weightModel
    )
    {
        if (
            !_tileIndexParser
                .Parse(weightModel.Tile)
                .TryPickT0(out var tileIndex, out var tileError)
        )
        {
            return tileError;
        }

        if (weightModel.Weight is not { } weight)
        {
            return FileParsingError.New("Weight is required.");
        }

        return new WeightConfiguration.TileWeight { Tile = tileIndex, Weight = weight };
    }
}