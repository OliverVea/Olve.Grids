using Olve.Grids.Grids;
using UI.Core.Projects;

namespace UI.Blazor.Navigation;

public abstract class NavigationTargets
{
    public readonly record struct Home : INavigationTarget
    {
        public Url GetUrl() => new("/");
    }

    public readonly record struct CreateProject : INavigationTarget
    {
        public Url GetUrl() => new("/create-project");
    }

    public readonly record struct EditTileAdjacencies(Id<Project> ProjectId, TileIndex TileIndex) : INavigationTarget
    {
        public Url GetUrl() => new($"project/{ProjectId}/edit-adjacencies/{TileIndex.Index}");
    }

    public readonly record struct ProjectDashboard(Id<Project> ProjectId) : INavigationTarget
    {
        public Url GetUrl() => new($"/project/{ProjectId}");
    }
}