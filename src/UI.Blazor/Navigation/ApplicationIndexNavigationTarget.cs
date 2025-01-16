namespace UI.Blazor.Navigation;

public readonly record struct ApplicationIndexNavigationTarget : INavigationTarget
{
    public Url GetUrl() => new("/");
}