namespace Olve.Grids.IO.Configuration.Models;

internal class AdjacencyModel
{
    /// <example>16</example>
    public int? Tile { get; set; }
    
    /// <example>right</example>
    public string[]? OverwriteBrushAdjacencies { get; set; }
    
    public AdjacentModel[]? Adjacents { get; set; }
}