namespace UI.Blazor.Interop;

public readonly record struct IdSelector(string Id) : IElementSelector
{
    public string Selector => $"#{Id}";
}