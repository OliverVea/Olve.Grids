using System.Text.Json;

namespace UI.Services.Projects.FileSystem;

public static class ProjectFileHelper
{
    public static Result Save(Project project, ProjectSummary projectSummary)
    {
        var directoryResult = EnsureDirectoryExists();
        if (!directoryResult.Succeded)
        {
            return directoryResult;
        }

        var projectResult = Save(project);
        if (!projectResult.Succeded)
        {
            return projectResult;
        }

        var projectSummaryResult = Save(projectSummary);
        if (projectSummaryResult.Succeded)
        {
            return projectSummaryResult;
        }

        return Result.Success();
    }

    private static Result EnsureDirectoryExists()
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

    private static Result Save(Project project)
    {
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

    private static Result Save(ProjectSummary projectSummary)
    {
        var projectSummaryFilePath = PathHelper.GetSummaryPath(projectSummary);
        var projectSummaryJson = JsonSerializer.Serialize(projectSummary, FileBasedJsonContext.Default.ProjectSummary);

        try
        {
            File.WriteAllText(projectSummaryFilePath, projectSummaryJson);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to write project summary file: {0}", projectSummaryFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }
}