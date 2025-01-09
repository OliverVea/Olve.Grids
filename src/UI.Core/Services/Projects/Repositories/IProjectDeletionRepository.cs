using Olve.Utilities.Operations;

namespace UI.Core.Services.Projects.Repositories;

public interface IProjectDeletionRepository
{
    Task<Result> DeleteProjectAsync(Id<Project> projectId, CancellationToken ct = default);
}

public interface IDeleteProjectRepositoryOperation : IAsyncOperation<
    IDeleteProjectRepositoryOperation.Request,
    Result>
{
    public record Request(Id<Project> ProjectId);
}