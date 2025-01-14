using MemoryPack;
using SixLabors.ImageSharp;

namespace UI.Core.Projects.FileSystem;

public static class ProjectFileHelper
{
    public static Result<Project> Load(string projectFile)
    {
        if (!File.Exists(projectFile))
        {
            var problem = new ResultProblem("Project file not found: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        byte[] projectBytes;
        try
        {
            projectBytes = File.ReadAllBytes(projectFile);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to read project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        SerializableProject? serializableProject;

        try
        {
            serializableProject = MemoryPackSerializer.Deserialize<SerializableProject>(projectBytes);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to deserialize project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        if (serializableProject is null)
        {
            var problem = new ResultProblem("Failed to deserialize project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        var projectId = Id<Project>.Parse(serializableProject.Id);
        var tileSheetImageExtension = Path.GetExtension(serializableProject.TileSheetImage.Name);
        Image tileSheetImage;
        try
        {
            var tileSheetImagePath = PathHelper.GetTileSheetPath(projectId, tileSheetImageExtension);
            tileSheetImage = Image.Load(tileSheetImagePath);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to read tile sheet image file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        Project project;

        try
        {
            project = serializableProject.ToProject(tileSheetImage);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex,
                "Failed to convert serializable project to project: {0}",
                serializableProject);
            return Result<Project>.Failure(problem);
        }


        return Result<Project>.Success(project);
    }


    public static Result Save(Project project)
    {
        var directoryResult = ProjectDirectoryHelper.EnsureDirectoryExists();
        if (directoryResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        SerializableProject serializableProject;
        Image tileSheetImage;

        try
        {
            var result = SerializableProject.FromProject(project);
            serializableProject = result.Project;
            tileSheetImage = result.Image;
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to convert project to serializable project: {0}", project);
            return Result.Failure(problem);
        }

        byte[] projectBytes;

        try
        {
            projectBytes = MemoryPackSerializer.Serialize(serializableProject);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to serialize project: {0}", project);
            return Result.Failure(problem);
        }

        var projectFilePath = PathHelper.GetProjectPath(project);
        try
        {
            File.WriteAllBytes(projectFilePath, projectBytes);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to write project file: {0}", projectFilePath);
            return Result.Failure(problem);
        }

        var tileSheetExtension = Path.GetExtension(project.TileSheetImage.Name);
        var imageFilePath = PathHelper.GetTileSheetPath(project.Id, tileSheetExtension);
        try
        {
            tileSheetImage.Save(imageFilePath);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to write tile sheet image file: {0}", imageFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }

    public static Result Delete(Project project)
    {
        var projectFilePath = PathHelper.GetProjectPath(project);
        try
        {
            File.Delete(projectFilePath);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to delete project file: {0}", projectFilePath);
            return Result.Failure(problem);
        }

        var tileSheetExtension = Path.GetExtension(project.TileSheetImage.Name);
        var imageFilePath = PathHelper.GetTileSheetPath(project.Id, tileSheetExtension);
        try
        {
            File.Delete(imageFilePath);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to delete tile sheet image file: {0}", imageFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }
}