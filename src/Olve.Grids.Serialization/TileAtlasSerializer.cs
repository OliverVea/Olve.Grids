using MemoryPack;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Grids.Weights;
using Olve.Utilities.Assertions;

namespace Olve.Grids.Serialization;

public class TileAtlasSerializer
{
    public byte[] Serialize(TileAtlas tileAtlas)
    {
        var serializableTileAtlas = Models.SerializableTileAtlas.FromTileAtlas(tileAtlas);

        return MemoryPackSerializer.Serialize(serializableTileAtlas);
    }

    public TileAtlas? Deserialize(byte[] data)
    {
        var serializableTileAtlas = MemoryPackSerializer.Deserialize<Models.SerializableTileAtlas>(data);
        if (serializableTileAtlas == null)
        {
            return null;
        }

        return Models.SerializableTileAtlas.ToTileAtlas(serializableTileAtlas);
    }
}