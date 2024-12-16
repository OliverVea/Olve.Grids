using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration;

public class TileGroups
{
    public Dictionary<string, IEnumerable<TileIndex>> Groups { get; init; } = new();
}