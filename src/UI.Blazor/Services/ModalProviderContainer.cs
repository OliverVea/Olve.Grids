using UI.Blazor.Components.Components;
using UI.Blazor.Components.Providers;

namespace UI.Blazor.Services;

public class ModalProviderContainer
{
    private ModalProvider? _provider;

    public ModalProvider Provider => _provider ?? throw new InvalidOperationException("ModalProvider has not been set.");

    public void SetProvider(ModalProvider provider) => _provider = provider;
}