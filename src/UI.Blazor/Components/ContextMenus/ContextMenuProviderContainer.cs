namespace UI.Blazor.Components.ContextMenus;

public class ContextMenuProviderContainer
{
    private ContextMenuProvider? _provider;

    public ContextMenuProvider Provider => _provider ?? throw new InvalidOperationException("Provider not set");

    public void SetProvider(ContextMenuProvider provider)
    {
        _provider = provider;
    }
}