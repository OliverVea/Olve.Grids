using Olve.Grids.Generation;
using OneOf.Types;

namespace Olve.Grids.Serialization;

public class TileAtlasFileLoader(ITileAtlasSerializer tileAtlasSerializer)
{
    public OneOf<Success, Error<string>> Save(TileAtlas tileAtlas, string path, bool overwrite = false)
    {
        if (File.Exists(path) && !overwrite)
        {
            return new Error<string>($"File already exists at {path}");
        }

        var data = tileAtlasSerializer.Serialize(tileAtlas);

        File.WriteAllBytes(path, data);

        return new Success();
    }

    public OneOf<TileAtlas, Error<string>> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new Error<string>($"File not found at {path}");
        }

        var data = File.ReadAllBytes(path);

        var tileAtlas = tileAtlasSerializer.Deserialize(data);

        if (tileAtlas == null)
        {
            return new Error<string>("Failed to deserialize tile atlas");
        }

        return tileAtlas;
    }
}