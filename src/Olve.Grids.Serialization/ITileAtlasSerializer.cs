using Olve.Grids.Generation;

namespace Olve.Grids.Serialization;

public interface ITileAtlasSerializer
{
    byte[] Serialize(TileAtlas tileAtlas);
    TileAtlas? Deserialize(byte[] data);
}