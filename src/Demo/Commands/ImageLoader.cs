using SixLabors.ImageSharp;

namespace Demo.Commands;

public class ImageLoader
{
    public Result<Image> LoadImage(string path)
    {
        try
        {
            var image = Image.Load(path);
            return Result<Image>.Success(image);
        }
        catch (Exception ex)
        {
            return Result<Image>.Failure(new ResultProblem(ex, "Failed to load image: {0}", path));
        }
    }
}