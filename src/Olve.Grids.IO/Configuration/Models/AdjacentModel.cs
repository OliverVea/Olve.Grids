namespace Olve.Grids.IO.Configuration.Models;

internal class AdjacentModel
{
    /// <example>16, 23-25</example>
    public string? Tiles { get; set; }

    /// <example>water</example>
    public string? Group { get; set; }

    /// <example>false</example>
    public bool IsAdjacent { get; set; } = true;

    /// <example>right</example>
    public string? Direction { get; set; }
}