namespace UI.Core.Services.Projects;

public interface ICurrentProjectRepository
{
    ValueTask<Result<Project?>> GetCurrentProjectAsync(CancellationToken ct = new());
    ValueTask<Result> SetCurrentProjectAsync(Project project, CancellationToken ct = new());
    ValueTask<Result> UpdateCurrentProjectAsync(Action<Project> update, CancellationToken ct = new());
    ISet<Func<Project, Task>> OnCurrentProjectChanged { get; }
}