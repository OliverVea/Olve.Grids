using UI.Core.Projects;

namespace UI.Blazor.Navigation;

public readonly record struct ProjectDashboardNavigationTarget(Id<Project> ProjectId) : INavigationTarget
{
    public Url GetUrl() => new($"/project/{ProjectId.ToString()}");
}