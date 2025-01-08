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

    private const int ImageHeaderStart = 16;

    public static Size GetImageSize(string fileName)
    {
        var br = new BinaryReader(File.OpenRead(fileName));

        br.BaseStream.Position = ImageHeaderStart;
        Span<byte> span = stackalloc byte[sizeof(int) * 2];

        for (var i = 0; i < sizeof(int) * 2; i++)
        {
            span[sizeof(int) * 2 - 1 - i] = br.ReadByte();
        }

        return GetSizeFromSpan(span);
    }

    public static Size GetImageSize(byte[] data)
    {
        Span<byte> span = stackalloc byte[sizeof(int) * 2];

        for (var i = 0; i < sizeof(int) * 2; i++)
        {
            span[sizeof(int) * 2 - 1 - i] = data[i + ImageHeaderStart];
        }

        return GetSizeFromSpan(span);
    }

    private static Size GetSizeFromSpan(ReadOnlySpan<byte> span)
    {
        var height = BitConverter.ToInt32(span[..sizeof(int)]);
        var width = BitConverter.ToInt32(span.Slice(sizeof(int), sizeof(int)));

        return new Size(width, height);
    }
}