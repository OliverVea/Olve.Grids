using Olve.Utilities.AsyncOnStartup;
using UI.Core;
using UI.Core.Projects;

namespace UI.Blazor.Components.ProjectDashboard;

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