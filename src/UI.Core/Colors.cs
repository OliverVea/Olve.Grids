using Wacton.Unicolour;

namespace UI.Core;

public static class Colors
{
    public static readonly ColorString Transparent = new("transparent");

    // https://m2.material.io/design/color/dark-theme.html#properties
    private static readonly ColorString Background = new("#181818");
    public static readonly ColorString White = new("#ffffff");
    public static readonly ColorString Red = new("#FF0000");
    private static readonly ColorString BrandColor = new("#624E88");
    private static readonly ColorString BrandedBackground = Overlay(BrandColor, Background, 0.08f);

    private static readonly ColorScheme Dark = new()
    {
        Text = new ColorSchemeText
        {
            Danger = Overlay(Red, Background, 0.87f),
            Less = Overlay(White, BrandedBackground, 0.38f),
            Ordinary = Overlay(White, BrandedBackground, 0.60f),
            More = Overlay(White, BrandedBackground, 0.87f),
            Most = Overlay(White, BrandedBackground, 1f),
        },
        Panels = new ColorSchemePanels
        {
            Ordinary = BrandedBackground,
            More = Overlay(White, BrandedBackground, 0.05f),
            Most = Overlay(White, BrandedBackground, 0.07f),
        },
    };

    public static ColorScheme Active = Dark;

    private static ColorString Overlay(ColorString foreground, ColorString background, float alpha)
    {
        Unicolour foregroundColor = (Unicolour)foreground, backgroundColor = (Unicolour)background;

        var overlay = backgroundColor.Mix(foregroundColor, ColourSpace.Rgb, alpha);

        return (ColorString)overlay;
    }
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
    public ColorString Danger { get; init; } = ColorString.NotSet;
    public ColorString Less { get; init; } = ColorString.NotSet;
    public ColorString Ordinary { get; init; } = ColorString.NotSet;
    public ColorString More { get; init; } = ColorString.NotSet;
    public ColorString Most { get; init; } = ColorString.NotSet;
}

public readonly record struct ColorString(string Value)
{
    public static readonly ColorString NotSet = new("#FFC0CB");

    public static explicit operator Unicolour(ColorString colorString) => new(colorString.Value);
    public static explicit operator ColorString(Unicolour unicolour) => new(unicolour.Hex);
}