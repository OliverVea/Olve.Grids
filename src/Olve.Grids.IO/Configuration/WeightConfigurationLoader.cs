using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Weights;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class WeightConfigurationLoader(WeightConfigurationParser weightConfigurationParser)
{
    public OneOf<IWeightLookupBuilder, FileParsingError> LoadWeightConfiguration(
        ConfigurationModel configurationModel,
        IEnumerable<TileIndex> tileIndices
    )
    {
        var weightLookupBuilder = new WeightLookup();

        var result = ConfigureWeightLookupBuilder(configurationModel, weightLookupBuilder, tileIndices);

        return result.Match<OneOf<IWeightLookupBuilder, FileParsingError>>(
            _ => weightLookupBuilder,
            error => error
        );
    }

    public OneOf<Success, FileParsingError> ConfigureWeightLookupBuilder(
        ConfigurationModel configurationModel,
        IWeightLookupBuilder weightLookupBuilder,
        IEnumerable<TileIndex> tileIndices,
        float defaultWeight = 1.0f)

    {
        foreach (var tileIndex in tileIndices)
        {
            weightLookupBuilder.SetWeight(tileIndex, defaultWeight);
        }

        if (!weightConfigurationParser
                .Parse(configurationModel)
                .TryPickT0(out var weightConfiguration, out var parsingError))
        {
            return parsingError;
        }

        foreach (var tileWeight in weightConfiguration.Weights)
        {
            foreach (var tile in tileWeight.Tiles)
            {
                var currentWeight = weightLookupBuilder.GetWeight(tile);
                var newWeight = tileWeight.WeightFunction(currentWeight);

                weightLookupBuilder.SetWeight(tile, newWeight);
            }
        }

        return new Success();
    }
}