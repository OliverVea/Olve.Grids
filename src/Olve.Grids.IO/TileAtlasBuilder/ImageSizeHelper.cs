namespace Olve.Grids.IO.TileAtlasBuilder;

public static class ImageSizeHelper
{
    public static bool TryGetImageSize(string fileName, out Size imageSize)
    {
        try
        {
            imageSize = GetImageSize(fileName);
            return true;
        }
        catch
        {
            imageSize = new Size();
            return false;
        }
    }

    public static Size GetImageSize(string fileName)
    {
        var br = new BinaryReader(File.OpenRead(fileName));

        br.BaseStream.Position = 16;
        var widthBytes = new byte[sizeof(int)];
        for (var i = 0; i < sizeof(int); i++)
        {
            widthBytes[sizeof(int) - 1 - i] = br.ReadByte();
        }

        var width = BitConverter.ToInt32(widthBytes, 0);

        var heightBytes = new byte[sizeof(int)];
        for (var i = 0; i < sizeof(int); i++)
        {
            heightBytes[sizeof(int) - 1 - i] = br.ReadByte();
        }

        var height = BitConverter.ToInt32(heightBytes, 0);

        return new Size(width, height);
    }
}