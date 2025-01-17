namespace UI.Core;

public readonly record struct HsvNormal(NormalParameters Saturation, NormalParameters Value)
{
    public static readonly HsvNormal Default = new(
        new NormalParameters(0.5f, 0.2f),
        new NormalParameters(0.6f, 0.05f));
}