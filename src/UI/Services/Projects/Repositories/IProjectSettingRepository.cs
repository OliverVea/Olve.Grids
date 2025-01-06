namespace UI.Services.Projects.Repositories;

public interface IProjectSettingRepository
{
    Task<Result> SetProjectAsync(Project project, CancellationToken ct = default);
    Task<Result> SetProjectAsync(ProjectSummary projectSummary, CancellationToken ct = default);
    Task<Result> SetProjectAsync(Project project, ProjectSummary projectSummary, CancellationToken ct = default);
}