using System.Text.Json;

namespace UI.Services.Projects.FileSystem;

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
}