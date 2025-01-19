using Wacton.Unicolour;

namespace UI.Core;

public static class Colors
{
    private static readonly ColorString Background = new("#181818");
    private static readonly ColorString BrandColor = new("#624E88");
    private static readonly ColorString BrandedBackground = ColorString.Overlay(BrandColor, Background, 0.08f);

    private static readonly ColorScheme Dark = new()
    {
        Text = new ColorSchemeText
        {
            Danger = ColorString.Overlay(ColorString.Red, Background, 0.87f),
            Less = ColorString.Overlay(ColorString.White, BrandedBackground, 0.38f),
            Ordinary = ColorString.Overlay(ColorString.White, BrandedBackground, 0.60f),
            More = ColorString.Overlay(ColorString.White, BrandedBackground, 0.87f),
            Most = ColorString.Overlay(ColorString.White, BrandedBackground, 1f),
        },
        Panels = new ColorSchemePanels
        {
            Ordinary = BrandedBackground,
            More = ColorString.Overlay(ColorString.White, BrandedBackground, 0.05f),
            Most = ColorString.Overlay(ColorString.White, BrandedBackground, 0.07f),
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

    public static readonly ColorString Transparent = new("transparent");

    public static readonly ColorString White = new("#ffffff");
    public static readonly ColorString Red = new("#FF0000");
    public static readonly ColorString Green = new("#00FF00");
    public static readonly ColorString Blue = new("#0000FF");
    public static readonly ColorString Black = new("#000000");
    public static readonly ColorString Black50 = new("#00000080");
    public static readonly ColorString Black25 = new("#00000040");
    public static readonly ColorString Black125 = new("#00000020");

    public static ColorString Overlay(ColorString foreground, ColorString background, float alpha)
    {
        Unicolour foregroundColor = (Unicolour)foreground, backgroundColor = (Unicolour)background;

        var overlay = backgroundColor.Mix(foregroundColor, ColourSpace.Rgb, alpha);

        return (ColorString)overlay;
    }
}