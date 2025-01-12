using Microsoft.AspNetCore.Components;

namespace UI.Blazor.Components.Providers;

public class ModalService(ModalProviderContainer providerContainer)
{
    private ModalProvider Provider => providerContainer.Provider;

    public void Register(ModalProvider modalProvider)
    {
        providerContainer.SetProvider(modalProvider);
    }

    public async Task ShowAsync(RenderFragment childContent, string? title = null)
    {
        await Provider.MutateAsync(state =>
        {
            state.Title = title;
            state.ChildContent = childContent;
            state.Visible = true;
        });
    }
}