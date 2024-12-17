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

    public static GridConfiguration ToGridConfiguration(SerializableGridConfiguration serializableGridConfiguration) =>
        new(new Size(serializableGridConfiguration.TileWidth, serializableGridConfiguration.TileHeight),
            serializableGridConfiguration.Columns,
            serializableGridConfiguration.Rows);
}