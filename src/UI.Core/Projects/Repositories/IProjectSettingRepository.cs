namespace UI.Core.Projects.Repositories;

public interface IProjectSettingRepository
{
    Task<Result> SetProjectAsync(Project project, CancellationToken ct = default);
    Task<Result> SetProjectSummaryAsync(ProjectSummary projectSummary, CancellationToken ct = default);
    Task<Result> SetProjectAndSummaryAsync(Project project, ProjectSummary projectSummary, CancellationToken ct = default);
}