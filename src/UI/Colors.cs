namespace UI;

public static class Colors
{
    public const string Transparent = "transparent";

    public static readonly ColorScheme Dark = new()
    {
        Text = new ColorSchemeText
        {
            Ordinary = "#6e7270",
            More = "#cccccc",
            Most = "#ffffff",
        },
        Panels = new ColorSchemePanels
        {
            Ordinary = "#1e1f22",
            More = "#2b2d30",
            Most = "#3c3f41",
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
    public string Ordinary { get; init; } = "pink";
    public string More { get; init; } = "pink";
    public string Most { get; init; } = "pink";
}

public class ColorSchemeText
{
    public string Ordinary { get; init; } = "pink";
    public string More { get; init; } = "pink";
    public string Most { get; init; } = "pink";
}