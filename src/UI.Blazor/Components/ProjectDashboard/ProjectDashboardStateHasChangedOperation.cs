using Olve.Utilities.Operations;

namespace UI.Blazor.Components.ProjectDashboard;

public class ProjectDashboardStateHasChangedOperation(ProjectDashboardService projectDashboardService)
    : IAsyncOperation<ProjectDashboardStateHasChangedOperation.Request>
{
    public record Request;

    public class Factory(IServiceProvider serviceProvider)
        : AsyncOperationFactory<ProjectDashboardStateHasChangedOperation, Request>(serviceProvider);

    public Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        return projectDashboardService
            .StateHasChangedAsync(ct)
            .ContinueWith(_ => Result.Success(), ct);
    }
}