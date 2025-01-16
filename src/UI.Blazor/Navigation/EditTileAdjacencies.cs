using UI.Core.Projects;

namespace UI.Blazor.Navigation;

public readonly record struct EditTileAdjacencies(Id<Project> ProjectId) : INavigationTarget
{
    public Url GetUrl() => new($"/project/{ProjectId}/edit-tile-adjacencies");
}