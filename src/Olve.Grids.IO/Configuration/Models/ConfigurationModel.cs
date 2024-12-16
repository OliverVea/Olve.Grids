namespace Olve.Grids.IO.Configuration.Models;

/// <summary>
/// The model of the configuration file.
/// Parsed into <see cref="IOConfiguration"/>.
/// </summary>
public class ConfigurationModel
{
    internal bool GenerateAdjacenciesFromBrushes { get; set; } = true;
    internal AdjacencyModel[]? Adjacencies { get; set; }
    internal WeightModel[]? Weights { get; set; }
}