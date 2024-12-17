using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Weights;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class WeightConfigurationLoader(WeightConfigurationParser weightConfigurationParser)
{
    public OneOf<IWeightLookup, FileParsingError> LoadWeightConfiguration(
        ConfigurationModel configurationModel,
        IEnumerable<TileIndex> tileIndices
    )
    {
        var weightLookupBuilder = new WeightLookup();

        var result = ConfigureWeightLookupBuilder(configurationModel, weightLookupBuilder, tileIndices);

        return result.Match<OneOf<IWeightLookup, FileParsingError>>(
            _ => weightLookupBuilder,
            error => error
        );
    }

    public OneOf<Success, FileParsingError> ConfigureWeightLookupBuilder(
        ConfigurationModel configurationModel,
        IWeightLookup weightLookup,
        IEnumerable<TileIndex> tileIndices,
        float defaultWeight = 1.0f)

    {
        foreach (var tileIndex in tileIndices)
        {
            weightLookup.SetWeight(tileIndex, defaultWeight);
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
                var currentWeight = weightLookup.GetWeight(tile);
                var newWeight = tileWeight.WeightFunction(currentWeight);

                weightLookup.SetWeight(tile, newWeight);
            }
        }

        return new Success();
    }
}