using Olve.Utilities.Operations;

namespace UI.Core.Services.Projects;

public class SetCurrentProjectOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<SetCurrentProjectOperation.Request,
        Result>
{
    public record Request(Project Project);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new()) =>
        await currentProjectRepository.SetCurrentProjectAsync(request.Project, ct);
}