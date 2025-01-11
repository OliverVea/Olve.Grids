using Olve.Grids.IO.Configuration.Models;

namespace Olve.Grids.IO.Configuration.Parsing;

public class WeightConfigurationParser(TileGroupParser tileGroupParser) : IParser<WeightConfiguration>
{
    public Result<WeightConfiguration> Parse(ConfigurationModel configurationModel)
    {
        if (ParseWeights(configurationModel).TryPickProblems(out var problems, out var weights))
        {
            return problems;
        }

        return new WeightConfiguration
        {
            Weights = weights,
        };
    }

    private Result<IReadOnlyList<WeightConfiguration.TileWeight>> ParseWeights(ConfigurationModel configurationModel)
    {
        if (configurationModel.Weights is not { } weightModels)
        {
            return Array.Empty<WeightConfiguration.TileWeight>();
        }
        
        // Todo: Parsing tile groups here too?
        var tileGroupParsingResult = tileGroupParser.Parse(configurationModel);
        if (tileGroupParsingResult.TryPickProblems(out var problems, out var tileGroups))
        {
            return problems;
        }

        var weightParsingResults = weightModels.Select(x => ParseWeight(tileGroups, x));

        if (weightParsingResults.TryPickProblems(out problems, out var weights))
        {
            return problems;
        }

        return weights.ToArray();
    }

    private static readonly Dictionary<string, Func<float, Func<float, float>>> WeightFunctions = new()
    {
        ["set"] = weight => _ => weight,
        ["add"] = weight => currentWeight => currentWeight + weight,
        ["multiply"] = weight => currentWeight => currentWeight * weight,
    };

    private Result<Func<float, float>> ParseWeightFunction(WeightModel weightModel)
    {
        var mode = weightModel.Mode ?? "multiply";

        if (!WeightFunctions.TryGetValue(mode, out var weightFunction))
        {
            // Todo: Ensure that result problems are formatted the same
            //       - '' around parameters
            //       - No full stop
            //       Maybe use an analyzer?
            return new ResultProblem("Unknown weight mode '{0}'", mode);
        }

        if (weightModel.Weight is not { } weight)
        {
            return new ResultProblem("Weight is required");
        }

        return weightFunction(weight);
    }

    private Result<WeightConfiguration.TileWeight> ParseWeight(
        TileGroups tileGroups,
        WeightModel weightModel
    )
    {
        var tileGroupResults = tileGroupParser.Parse(weightModel.Tiles, weightModel.Group, tileGroups);
        if (tileGroupResults.TryPickProblems(out var problems, out var tileIndices))
        {
            return problems;
        }

        var weightFunctionResult = ParseWeightFunction(weightModel);
        if (weightFunctionResult.TryPickProblems(out problems, out var weightFunction))
        {
            return problems;
        }

        return new WeightConfiguration.TileWeight
        {
            Tiles = tileIndices.ToArray(),
            WeightFunction = weightFunction,
        };
    }
}