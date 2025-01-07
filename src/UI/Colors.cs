namespace UI;

public static class Colors
{
    public static readonly ColorString Transparent = new("transparent");

    private static readonly ColorScheme Dark = new()
    {
        Text = new ColorSchemeText
        {
            Less = new ColorString("#585B59"),
            Ordinary = new ColorString("#6e7270"),
            More = new ColorString("#cccccc"),
            Most = new ColorString("#ffffff"),
        },
        Panels = new ColorSchemePanels
        {
            Ordinary = new ColorString("#1e1f22"),
            More = new ColorString("#2b2d30"),
            Most = new ColorString("#3c3f41"),
        },
    };

    public static ColorScheme Active = Dark;
}

public class ColorScheme
{
    public required ColorSchemeText Text { get; init; }
    public required ColorSchemePanels Panels { get; init; }
}

public class ColorSchemePanels
{
    public ColorString Ordinary { get; init; } = ColorString.NotSet;
    public ColorString More { get; init; } = ColorString.NotSet;
    public ColorString Most { get; init; } = ColorString.NotSet;
}

public class ColorSchemeText
{
    public ColorString Less { get; init; } = ColorString.NotSet;
    public ColorString Ordinary { get; init; } = ColorString.NotSet;
    public ColorString More { get; init; } = ColorString.NotSet;
    public ColorString Most { get; init; } = ColorString.NotSet;
}

public readonly record struct ColorString(string Value)
{
    public static readonly ColorString NotSet = new("pink");
}