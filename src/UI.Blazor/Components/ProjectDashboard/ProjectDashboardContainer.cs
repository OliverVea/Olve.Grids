namespace UI.Blazor.Components.ProjectDashboard;

public class ProjectDashboardContainer
{
    private Pages.ProjectDashboard? _provider;

    public Pages.ProjectDashboard Provider =>
        _provider ?? throw new InvalidOperationException("ModalProvider has not been set.");

    public void SetProvider(Pages.ProjectDashboard provider) => _provider = provider;
}