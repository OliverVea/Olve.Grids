using Microsoft.AspNetCore.Components;

namespace UI.Blazor.Navigation;

public class NavigationService(NavigationManager navigationManager)
{
    public Result NavigateTo(INavigationTarget navigationTarget)
    {
        var uri = navigationTarget.GetUrl();
        var uriString = uri.Value;

        navigationManager.NavigateTo(uriString);

        return Result.Success();
    }
}