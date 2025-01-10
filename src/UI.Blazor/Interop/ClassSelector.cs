namespace UI.Blazor.Interop;

public readonly record struct ClassSelector(string Class) : IElementSelector
{
    public string Selector => $".{Class}";
}