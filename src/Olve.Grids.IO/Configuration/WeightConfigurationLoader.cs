using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Weights;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class WeightConfigurationLoader
{
    private readonly WeightConfigurationParser _weightConfigurationParser = new();

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

        if (!_weightConfigurationParser
                .Parse(configurationModel)
                .TryPickT0(out var weightConfiguration, out var parsingError))
        {
            return parsingError;
        }

        foreach (var tileWeight in weightConfiguration.Weights)
        {
            weightLookupBuilder.SetWeight(tileWeight.Tile, tileWeight.Weight);
        }

        return new Success();
    }
}