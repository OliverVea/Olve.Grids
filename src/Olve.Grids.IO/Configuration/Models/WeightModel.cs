namespace Olve.Grids.IO.Configuration.Models;

internal class WeightModel
{
    /// <example>16, 23-25</example>
    public string? Tiles { get; set; }

    /// <example>water</example>
    public string? Group { get; set; }

    /// <example>1</example>
    public float? Weight { get; set; }

    /// <example>additive</example>
    public string? Mode { get; set; }
}