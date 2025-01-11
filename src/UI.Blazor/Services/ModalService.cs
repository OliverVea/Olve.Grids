using Microsoft.AspNetCore.Components;
using UI.Blazor.Components.Components;

namespace UI.Blazor.Services;

public class ModalProviderContainer
{
    private ModalProvider? _provider = null;

    public ModalProvider Provider => _provider ?? throw new InvalidOperationException("ModalProvider has not been set.");

    public void SetProvider(ModalProvider provider) => _provider = provider;
}

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