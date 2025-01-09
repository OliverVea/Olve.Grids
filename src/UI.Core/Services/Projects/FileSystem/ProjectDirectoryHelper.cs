namespace UI.Core.Services.Projects.FileSystem;

public static class ProjectDirectoryHelper
{
    public static Result EnsureDirectoryExists()
    {
        var directory = PathHelper.ProjectsFolder;

        try
        {
            Directory.CreateDirectory(directory);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to create directory: {0}", directory);
            return Result.Failure(problem);
        }

        return Result.Success();
    }
}