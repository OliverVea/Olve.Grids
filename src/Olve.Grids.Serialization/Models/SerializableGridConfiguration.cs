using MemoryPack;
using Olve.Grids.Grids;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableGridConfiguration
{
    public int Rows { get; set; }

    public int Columns { get; set; }

    public int TileWidth { get; set; }
    public int TileHeight { get; set; }

    public static SerializableGridConfiguration FromGridConfiguration(GridConfiguration gridConfiguration) =>
        new()
        {
            TileWidth = gridConfiguration.TileSize.Width,
            TileHeight = gridConfiguration.TileSize.Height,
            Columns = gridConfiguration.Columns,
            Rows = gridConfiguration.Rows,
        };

    public GridConfiguration ToGridConfiguration() => new(new Size(TileWidth, TileHeight), Rows, Columns);
}