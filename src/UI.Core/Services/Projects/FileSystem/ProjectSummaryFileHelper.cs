using System.Text.Json;

namespace UI.Core.Services.Projects.FileSystem;

public static class ProjectSummaryFileHelper
{
    public static Result<ProjectSummary> Load(string file)
    {
        if (!File.Exists(file))
        {
            var problem = new ResultProblem("Project summary file not found: {0}", file);
            return Result<ProjectSummary>.Failure(problem);
        }

        string json;
        try
        {
            json = File.ReadAllText(file);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to read project summary file: {0}", file);
            return Result<ProjectSummary>.Failure(problem);
        }

        ProjectSummary? projectSummary;

        try
        {
            projectSummary =
                JsonSerializer.Deserialize<ProjectSummary>(json, FileBasedJsonContext.Default.ProjectSummary);
        }
        // TODO: Handle specific exceptions
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to deserialize project summary file: {0}", file);
            return Result<ProjectSummary>.Failure(problem);
        }

        if (projectSummary is null)
        {
            var problem = new ResultProblem("Failed to deserialize project summary file: {0}", file);
            return Result<ProjectSummary>.Failure(problem);
        }

        return Result<ProjectSummary>.Success(projectSummary);
    }

    public static Result Save(ProjectSummary projectSummary)
    {
        var directoryResult = ProjectDirectoryHelper.EnsureDirectoryExists();
        if (!directoryResult.Succeded)
        {
            return directoryResult;
        }

        var projectFilePath = PathHelper.GetSummaryPath(projectSummary);

        string projectJson;

        try
        {
            projectJson = JsonSerializer.Serialize(projectSummary, FileBasedJsonContext.Default.ProjectSummary);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to serialize project summary: {0}", projectSummary);
            return Result.Failure(problem);
        }

        try
        {
            File.WriteAllText(projectFilePath, projectJson);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to write project summary file: {0}", projectFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }

    public static Result Delete(ProjectSummary projectSummary)
    {
        var projectFilePath = PathHelper.GetSummaryPath(projectSummary);

        try
        {
            File.Delete(projectFilePath);
        }
        catch (Exception ex)
        {
            var problem = new ResultProblem(ex, "Failed to delete project summary file: {0}", projectFilePath);
            return Result.Failure(problem);
        }

        return Result.Success();
    }
}