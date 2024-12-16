namespace Olve.Grids.IO.Configuration.Models;

internal class AdjacencyModel
{
    /// <example>16, 23-25</example>
    public string? Tiles { get; set; }

    /// <example>water</example>
    public string? Group { get; set; }

    /// <example>right</example>
    public string[]? OverwriteBrushAdjacencies { get; set; }

    public AdjacentModel[]? Adjacents { get; set; }
}