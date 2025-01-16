using Olve.Utilities.AsyncOnStartup;
using Olve.Utilities.Operations;
using UI.Core;
using UI.Core.Projects;

namespace UI.Blazor.Components.Pages;

public class ProjectDashboardContainer
{
    private ProjectDashboardRoot? _provider;

    public ProjectDashboardRoot Provider =>
        _provider ?? throw new InvalidOperationException("ModalProvider has not been set.");

    public void SetProvider(ProjectDashboardRoot provider) => _provider = provider;
}

public class ProjectDashboardService(ProjectDashboardContainer projectDashboardContainer)
{
    public Task StateHasChangedAsync(CancellationToken ct = default) =>
        projectDashboardContainer.Provider.StateHasChangedAsync();
}

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

public class RegisterProjectDashboardStateChangeOnStartup(ProjectDashboardStateHasChangedOperation.Factory operationFactory)
    : IAsyncOnStartup
{

    public Task OnStartupAsync(CancellationToken cancellationToken = default)
    {
        Events.ProjectChanged += OnProjectChanged;

        return Task.CompletedTask;
    }

    private void OnProjectChanged(Id<Project> projectId)
    {
        var operation = operationFactory.Build();

        ProjectDashboardStateHasChangedOperation.Request request = new();
        operation.ExecuteAsync(request);
    }
}