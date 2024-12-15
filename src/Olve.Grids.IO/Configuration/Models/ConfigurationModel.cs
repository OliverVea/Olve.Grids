namespace Olve.Grids.IO.Configuration.Models;

/// <summary>
/// The model of the configuration file.
/// Parsed into <see cref="IOConfiguration"/>.
/// </summary>
internal class ConfigurationModel
{
    public bool GenerateAdjacenciesFromBrushes { get; set; } = true;
    public AdjacencyModel[]? Adjacencies { get; set; }
    public WeightModel[]? Weights { get; set; }
}