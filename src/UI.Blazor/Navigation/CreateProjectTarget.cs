namespace UI.Blazor.Navigation;

public readonly record struct CreateProjectTarget : INavigationTarget
{
    public Url GetUrl() => new("/create-project");
}