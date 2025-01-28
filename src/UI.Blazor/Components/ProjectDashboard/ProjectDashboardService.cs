namespace UI.Blazor.Components.ProjectDashboard;

public class ProjectDashboardService(ProjectDashboardContainer projectDashboardContainer)
{
    public Task StateHasChangedAsync(CancellationToken ct = default)
    {
        if (projectDashboardContainer.Provider is { } provider)
        {
            return provider.StateHasChangedAsync();
        }

        return Task.CompletedTask;
    }
}