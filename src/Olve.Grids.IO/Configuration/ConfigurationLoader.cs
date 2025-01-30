using Microsoft.Extensions.DependencyInjection;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Weights;


namespace Olve.Grids.IO.Configuration;

public class ConfigurationLoader(
    AdjacencyConfigurationLoader adjacencyLoader,
    ConfigurationModelFileReader configurationModelFileReader,
    WeightConfigurationLoader weightLoader)
{
    public static ConfigurationLoader Create()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<AdjacencyConfigurationParser>();
        serviceCollection.AddSingleton<WeightConfigurationParser>();
        serviceCollection.AddSingleton<AdjacencyConfigurationLoader>();
        serviceCollection.AddSingleton<ConfigurationModelFileReader>();
        serviceCollection.AddSingleton<WeightConfigurationLoader>();
        serviceCollection.AddSingleton<TileGroupParser>();
        serviceCollection.AddSingleton<TileIndexParser>();
        // Todo: IL2066: Type passed to generic parameter 'TService' of
        //       'Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddSingleton<TService>(IServiceCollection)'
        //        can not be statically determined and may not meet 'DynamicallyAccessedMembersAttribute' requirements.
#pragma warning disable IL2066
        serviceCollection.AddSingleton<DirectionParser>();
#pragma warning restore IL2066
        serviceCollection.AddSingleton<ConfigurationLoader>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<ConfigurationLoader>();
    }


    public Result Load(string configurationFilePath,
        IAdjacencyLookup adjacencyLookup,
        IWeightLookup weightLookup,
        IEnumerable<TileIndex> tileIndices,
        TileBrushes brushConfiguration)
    {
        var configurationModelFileResult = configurationModelFileReader.Read(configurationFilePath);
        if (configurationModelFileResult.TryPickProblems(out var problems, out var configurationModel))
        {
            return problems;
        }

        var adjacencyResult = adjacencyLoader.ConfigureAdjacencyLookupBuilder(
            configurationModel,
            adjacencyLookup,
            brushConfiguration);
        if (adjacencyResult.TryPickProblems(out problems))
        {
            return problems;
        }

        var weightResult = weightLoader.ConfigureWeightLookupBuilder(configurationModel, weightLookup, tileIndices);
        if (weightResult.TryPickProblems(out problems))
        {
            return problems;
        }

        return Result.Success();
    }
}