using Olve.Utilities.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class SetCurrentProjectOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<SetCurrentProjectOperation.Request>
{
    public record Request(Project Project);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new()) =>
        await currentProjectRepository.SetCurrentProjectAsync(request.Project, ct);
}