using Olve.Utilities.Operations;

namespace UI.Core.Services.Projects;

public class UpdateCurrentProjectOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<UpdateCurrentProjectOperation.Request,
        Result>
{
    public record Request(Action<Project> Update);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var updateResult = await currentProjectRepository.UpdateCurrentProjectAsync(request.Update, ct);

        if (updateResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }
}