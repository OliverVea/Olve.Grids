namespace Olve.Grids.IO.Configuration.Models;

/// <summary>
///     The model of the configuration file.
/// </summary>
public class ConfigurationModel
{
    internal bool GenerateAdjacenciesFromBrushes { get; set; } = true;
    internal AdjacencyModel[]? Adjacencies { get; set; }
    internal WeightModel[]? Weights { get; set; }
    internal Dictionary<string, GroupModel>? Groups { get; set; }
}