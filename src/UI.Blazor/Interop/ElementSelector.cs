namespace UI.Blazor.Interop;

public readonly record struct ElementSelector(string Element) : IElementSelector
{
    public string Selector => Element;
}