using MemoryPack;
using Olve.Grids.Generation;

namespace Olve.Grids.Serialization;

public class TileAtlasSerializer : ITileAtlasSerializer
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