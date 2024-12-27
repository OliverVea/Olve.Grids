using Olve.Utilities.Paginations;

namespace UI.Services.Projects.FileSystem;

public class FileBasedProjectRepository : IProjectRepository
{
    public Task<Result<PaginatedResult<ProjectSummary>>> ListProjectSummariesAsync(Pagination pagination,
        CancellationToken ct = default) => Task.FromResult(ListProjectSummaries(pagination));

    private Result<PaginatedResult<ProjectSummary>> ListProjectSummaries(Pagination pagination)
    {
        var files = PathHelper
            .GetProjectSummaryFilePaths()
            .ToArray();

        var summaries = files
            .Skip(pagination.Page * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(ProjectSummaryFileHelper.Load)
            .ToArray();

        if (summaries.HasProblems())
        {
            var problems = summaries
                .GetProblems()
                .ToArray();

            return Result<PaginatedResult<ProjectSummary>>.Failure(problems);
        }

        var projectSummaries = summaries
            .GetValues()
            .ToArray();

        var totalCount = files.Length;

        return new PaginatedResult<ProjectSummary>(projectSummaries, pagination, totalCount);
    }

    public Task<Result<ProjectPath>> CreateProjectAsync(Project project, CancellationToken ct = default) =>
        Task.FromResult(CreateProject(project));

    private Result<ProjectPath> CreateProject(Project project)
    {
        var projectFilePathString = PathHelper.GetProjectPath(project);
        var projectFilePath = new ProjectPath(projectFilePathString);

        var projectSummary = new ProjectSummary(project.Id, project.Name, projectFilePath);

        var result = ProjectFileHelper.Save(project, projectSummary);
        if (result.TryPickProblems(out var problems))
        {
            return Result<ProjectPath>.Failure(problems);
        }

        return projectFilePath;
    }
}