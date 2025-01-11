using Olve.Grids.Generation;


namespace Olve.Grids.Serialization;

public class TileAtlasFileLoader(ITileAtlasSerializer tileAtlasSerializer)
{
    public Result Save(TileAtlas tileAtlas, string path, bool overwrite = false)
    {
        if (File.Exists(path) && !overwrite)
        {
            return new ResultProblem($"File already exists at {path}");
        }

        var data = tileAtlasSerializer.Serialize(tileAtlas);

        File.WriteAllBytes(path, data);

        return Result.Success();
    }

    public Result<TileAtlas> Load(string path)
    {
        if (!File.Exists(path))
        {
            return new ResultProblem($"File not found at {path}");
        }

        var data = File.ReadAllBytes(path);

        var tileAtlas = tileAtlasSerializer.Deserialize(data);

        if (tileAtlas == null)
        {
            return new ResultProblem("Failed to deserialize tile atlas");
        }

        return tileAtlas;
    }
}