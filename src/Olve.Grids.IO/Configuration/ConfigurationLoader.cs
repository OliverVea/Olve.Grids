using Microsoft.Extensions.DependencyInjection;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.Configuration.Parsing;
using Olve.Grids.Primitives;
using Olve.Grids.Weights;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class ConfigurationLoader(
    AdjacencyConfigurationLoader adjacencyConfigurationLoader,
    ConfigurationModelFileReader configurationModelFileReader,
    WeightConfigurationLoader weightConfigurationLoader)
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
        serviceCollection.AddSingleton<AdjacencyDirectionParser>();
        serviceCollection.AddSingleton<ConfigurationLoader>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        return serviceProvider.GetRequiredService<ConfigurationLoader>();
    }


    public OneOf<Success, FileParsingError> Load(string configurationFilePath,
        IAdjacencyLookup adjacencyLookup,
        IWeightLookup weightLookup,
        IEnumerable<TileIndex> tileIndices,
        IEnumerable<(TileIndex, Corner, BrushId)> brushConfiguration)
    {
        if (!configurationModelFileReader
                .Read(configurationFilePath)
                .TryPickT0(out var configurationModel, out var error))
        {
            return error;
        }

        if (!adjacencyConfigurationLoader
                .ConfigureAdjacencyLookupBuilder(configurationModel, adjacencyLookup, brushConfiguration)
                .TryPickT0(out _, out error))
        {
            return error;
        }

        if (!weightConfigurationLoader
                .ConfigureWeightLookupBuilder(configurationModel, weightLookup, tileIndices)
                .TryPickT0(out _, out error))
        {
            return error;
        }

        return new Success();
    }
}