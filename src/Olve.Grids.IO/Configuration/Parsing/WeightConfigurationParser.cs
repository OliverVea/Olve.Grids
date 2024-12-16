using Olve.Grids.IO.Configuration.Models;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

public class WeightConfigurationParser(TileGroupParser tileGroupParser) : IParser<WeightConfiguration>
{
    public OneOf<WeightConfiguration, FileParsingError> Parse(ConfigurationModel configurationModel)
    {
        if (!ParseWeights(configurationModel)
                .TryPickT0(out var weights, out var error))
        {
            return error;
        }

        return new WeightConfiguration
        {
            Weights = weights,
        };
    }

    private OneOf<IReadOnlyList<WeightConfiguration.TileWeight>, FileParsingError> ParseWeights(
        ConfigurationModel configurationModel)
    {
        if (configurationModel.Weights is not { } weightModels)
        {
            return Array.Empty<WeightConfiguration.TileWeight>();
        }


        if (!tileGroupParser
                .Parse(configurationModel)
                .TryPickT0(out var tileGroups, out var tileGroupsError))
        {
            return tileGroupsError;
        }

        var weightParsingResults = weightModels
            .Select(x => ParseWeight(tileGroups, x))
            .ToArray();

        if (weightParsingResults.AnyT1())
        {
            var errors = weightParsingResults.OfT1();
            return FileParsingError.Combine(errors);
        }

        return weightParsingResults
            .OfT0()
            .ToArray();
    }

    private static readonly Dictionary<string, Func<float, Func<float, float>>> WeightFunctions = new()
    {
        ["set"] = weight => _ => weight,
        ["add"] = weight => currentWeight => currentWeight + weight,
        ["multiply"] = weight => currentWeight => currentWeight * weight,
    };

    private OneOf<Func<float, float>, FileParsingError> ParseWeightFunction(WeightModel weightModel)
    {
        var mode = weightModel.Mode ?? "multiply";

        if (!WeightFunctions.TryGetValue(mode, out var weightFunction))
        {
            return FileParsingError.New($"Unknown weight mode '{mode}'.");
        }

        if (weightModel.Weight is not { } weight)
        {
            return FileParsingError.New("Weight is required.");
        }

        return weightFunction(weight);
    }

    private OneOf<WeightConfiguration.TileWeight, FileParsingError> ParseWeight(
        TileGroups tileGroups,
        WeightModel weightModel
    )
    {
        if (!tileGroupParser
                .Parse(weightModel.Tiles, weightModel.Group, tileGroups)
                .TryPickT0(out var tiles, out var tileError)
           )
        {
            return tileError;
        }

        if (!ParseWeightFunction(weightModel)
                .TryPickT0(out var weightFunction, out var weightFunctionError)
           )
        {
            return weightFunctionError;
        }

        return new WeightConfiguration.TileWeight
        {
            Tiles = tiles.ToArray(),
            WeightFunction = weightFunction,
        };
    }
}