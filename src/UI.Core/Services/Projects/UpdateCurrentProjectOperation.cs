using Olve.Utilities.Operations;

namespace UI.Core.Services.Projects;

public class UpdateCurrentProjectOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<UpdateCurrentProjectOperation.Request>
{
    public record Request(Func<Project, Result> Update)
    {
        public Request(Action<Project> Update) : this(p =>
        {
            Update(p);
            return Result.Success();
        })
        {
        }
    }

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var updateResult = await currentProjectRepository.UpdateCurrentProjectAsync(request.Update, ct);

        if (updateResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        return Result.Success();
    }
}