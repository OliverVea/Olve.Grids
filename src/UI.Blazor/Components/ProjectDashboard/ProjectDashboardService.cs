namespace UI.Blazor.Components.ProjectDashboard;

public class ProjectDashboardService(ProjectDashboardContainer projectDashboardContainer)
{
    public Task StateHasChangedAsync(CancellationToken ct = default) =>
        projectDashboardContainer.Provider.StateHasChangedAsync();
}