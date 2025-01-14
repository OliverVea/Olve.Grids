namespace UI.Core.Projects.Repositories;

public interface ICurrentProjectRepository
{
    ValueTask<Result<Project?>> GetCurrentProjectAsync(CancellationToken ct = new());
    ValueTask<Result> SetCurrentProjectAsync(Project project, CancellationToken ct = new());
    ValueTask<Result> UpdateCurrentProjectAsync(Func<Project, Result> update, CancellationToken ct = new());
    ISet<Func<Project, Task>> OnCurrentProjectChanged { get; }
}