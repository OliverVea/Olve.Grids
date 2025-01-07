using System.Text.Json;

namespace UI.Services.Projects.FileSystem;

public static class ProjectFileHelper
{
    public static Result<Project> Load(string projectFile)
    {
        if (!File.Exists(projectFile))
        {
            var problem = new ResultProblem("Project file not found: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        string json;
        try
        {
            json = File.ReadAllText(projectFile);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to read project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        Project? project;

        try
        {
            project = JsonSerializer.Deserialize<Project>(json, FileBasedJsonContext.Default.Project);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to deserialize project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        if (project is null)
        {
            var problem = new ResultProblem("Failed to deserialize project file: {0}", projectFile);
            return Result<Project>.Failure(problem);
        }

        return Result<Project>.Success(project);
    }


    public static Result Save(Project project)
    {
        var directoryResult = ProjectDirectoryHelper.EnsureDirectoryExists();
        if (!directoryResult.Succeded)
        {
            return directoryResult;
        }

        var projectFilePath = PathHelper.GetProjectPath(project);
        string projectJson;

        try
        {
            projectJson = JsonSerializer.Serialize(project, FileBasedJsonContext.Default.Project);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to serialize project: {0}", project);
            return Result.Failure(problem);
        }

        try
        {
            File.WriteAllText(projectFilePath, projectJson);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to write project file: {0}", projectFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }
}