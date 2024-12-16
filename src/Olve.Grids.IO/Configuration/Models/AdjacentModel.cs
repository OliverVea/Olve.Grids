namespace Olve.Grids.IO.Configuration.Models;

internal class AdjacentModel
{
    /// <example>16</example>
    public int? Tile { get; set; }

    /// <example>false</example>
    public bool IsAdjacent { get; set; } = true;

    /// <example>right</example>
    public string? Direction { get; set; }
}