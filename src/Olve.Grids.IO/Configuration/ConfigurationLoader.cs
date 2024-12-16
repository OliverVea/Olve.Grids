using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;
using OneOf.Types;

namespace Olve.Grids.IO.Configuration;

public class ConfigurationLoader
{
    private readonly AdjacencyConfigurationLoader _adjacencyConfigurationLoader = new();
    private readonly ConfigurationModelFileReader _configurationModelFileReader = new();
    private readonly WeightConfigurationLoader _weightConfigurationLoader = new();

    public OneOf<Success, FileParsingError> Load(string configurationFilePath,
        IAdjacencyLookupBuilder adjacencyLookupBuilder,
        IWeightLookupBuilder weightLookupBuilder,
        IEnumerable<TileIndex> tileIndices,
        IEnumerable<(TileIndex, Corner, OneOf<BrushId, Any>)> brushConfiguration)
    {
        if (!_configurationModelFileReader
                .Read(configurationFilePath)
                .TryPickT0(out var configurationModel, out var error))
        {
            return error;
        }

        if (!_adjacencyConfigurationLoader
                .ConfigureAdjacencyLookupBuilder(configurationModel, adjacencyLookupBuilder, brushConfiguration)
                .TryPickT0(out _, out error))
        {
            return error;
        }

        if (!_weightConfigurationLoader
                .ConfigureWeightLookupBuilder(configurationModel, weightLookupBuilder, tileIndices)
                .TryPickT0(out _, out error))
        {
            return error;
        }

        return new Success();
    }
}