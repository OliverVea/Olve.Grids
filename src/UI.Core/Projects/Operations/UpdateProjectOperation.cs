using Olve.Utilities.Operations;

namespace UI.Core.Projects.Operations;

public class UpdateProjectOperation(GetProjectOperation getProjectOperation, SetProjectOperation setProjectOperation)
    : IAsyncOperation<UpdateProjectOperation.Request>
{
    public record Request(Id<Project> ProjectId, Func<Project, Result> UpdateAction)
    {
        public Request(Id<Project> projectId, Action<Project> updateAction) : this(projectId, updateAction.ToFunc())
        {
        }
    }

    public async Task<Result> ExecuteAsync(Request input, CancellationToken ct = default)
    {
        GetProjectOperation.Request getProjectRequest = new(input.ProjectId);
        var projectResult = await getProjectOperation.ExecuteAsync(getProjectRequest, ct);
        if (projectResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        var updateResult = input.UpdateAction(project);
        if (updateResult.TryPickProblems(out problems))
        {
            return problems;
        }

        // Todo: check if project and summary were updated

        SetProjectOperation.Request setProjectRequest = new(project);
        var setProjectResult = await setProjectOperation.ExecuteAsync(setProjectRequest, ct);
        if (setProjectResult.TryPickProblems(out problems))
        {
            return problems;
        }

        return Result.Success();
    }

    private static ProjectSummary MapToSummary(Project project) => new(project.Id, project.Name, project.LastAccessedAt);
}