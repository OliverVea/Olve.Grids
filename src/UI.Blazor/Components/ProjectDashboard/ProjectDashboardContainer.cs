namespace UI.Blazor.Components.ProjectDashboard;

public class ProjectDashboardContainer
{
    public Pages.ProjectDashboard? Provider { get; private set; }

    public void SetProvider(Pages.ProjectDashboard provider) => Provider = provider;
}