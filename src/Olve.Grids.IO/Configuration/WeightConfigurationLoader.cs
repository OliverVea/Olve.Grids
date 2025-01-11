using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Models;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Weights;


namespace Olve.Grids.IO.Configuration;

public class WeightConfigurationLoader(WeightConfigurationParser weightConfigurationParser)
{
    public Result<IWeightLookup> LoadWeightConfiguration(ConfigurationModel configurationModel, IEnumerable<TileIndex> tileIndices)
    {
        var weightLookupBuilder = new WeightLookup();

        var result = ConfigureWeightLookupBuilder(configurationModel, weightLookupBuilder, tileIndices);
        if (result.TryPickProblems(out var problems))
        {
            return problems;
        }

        return weightLookupBuilder;
    }

    public Result ConfigureWeightLookupBuilder(
        ConfigurationModel configurationModel,
        IWeightLookup weightLookup,
        IEnumerable<TileIndex> tileIndices,
        float defaultWeight = 1.0f)

    {
        foreach (var tileIndex in tileIndices)
        {
            weightLookup.SetWeight(tileIndex, defaultWeight);
        }

        var parsingResult = weightConfigurationParser.Parse(configurationModel);
        if (parsingResult.TryPickProblems(out var problems, out var weightConfiguration))
        {
            return problems;
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

        return Result.Success();
    }
}