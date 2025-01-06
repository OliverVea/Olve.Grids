using Olve.Utilities.Operations;

namespace UI.Services.Projects;

public class CreateNewProjectOperation(IProjectRepository projectRepository) : IAsyncOperation<
    CreateNewProjectOperation.Request,
    Result<CreateNewProjectOperation.Response>>
{
    public record Request(string Name);

    public record Response(ProjectSummary ProjectSummary);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var id = Id<Project>.NewId();
        var projectName = new ProjectName(request.Name);
        var createdAt = DateTimeOffset.Now;

        var project = new Project(id, projectName, createdAt);

        var createResult = await projectRepository.SetProjectAsync(project, ct);
        if (createResult.TryPickProblems(out var problems, out var path))
        {
            return Result<Response>.Failure(problems);
        }

        var projectSummary = new ProjectSummary(id, projectName, path, createdAt);

        return new Response(projectSummary);
    }
}