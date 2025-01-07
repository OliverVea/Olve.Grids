namespace UI;

public static class Tw
{
    public static readonly TailwindClass Flex = new("flex");
    public static readonly TailwindClass FlexGrow = new("flex-grow");
    public static readonly TailwindClass FlexRow = new("flex-row");
    public static readonly TailwindClass FlexCol = new("flex-col");

    public static readonly TailwindClass Grid = new("grid");

    public static readonly TailwindClass ItemsCenter = new("items-center");
    public static readonly TailwindClass JustifyCenter = new("justify-center");
    public static readonly TailwindClass JustifyBetween = new("justify-between");
    public static readonly TailwindClass JustifyStart = new("justify-start");
    public static readonly TailwindClass JustifyEnd = new("justify-end");

    public static readonly TailwindClass HFull = new("h-full");
    public static readonly TailwindClass WFull = new("w-full");

    public static readonly TailwindClass BorderNone = new("border-none");
    public static readonly TailwindClass Rounded = new("rounded");
    public static readonly TailwindClass RoundedMd = new("rounded-md");

    public static readonly TailwindClass TextNoWrap = new("text-no-wrap");
    public static readonly TailwindClass TextLeft = new("text-left");

    public static readonly TailwindClass Underline = new("underline");

    public static readonly TailwindClass Transition = new("transition");

    public static readonly TailwindClass MinHScreen = new("min-h-screen");

    public static readonly TailwindClass FontThin = new("font-thin");
    public static readonly TailwindClass FontLight = new("font-light");
    public static readonly TailwindClass FontNormal = new("font-normal");
    public static readonly TailwindClass FontMedium = new("font-medium");
    public static readonly TailwindClass FontSemibold = new("font-semibold");
    public static readonly TailwindClass FontBold = new("font-bold");
    public static readonly TailwindClass Text8Xl = new("text-8xl");

    public static readonly TailwindClass OutlineNone = new("outline-none");
    public static readonly TailwindClass ItemsStart = new("items-start");
    public static readonly TailwindClass BreakWords = new("break-words");

    public static readonly TailwindClass CursorPointer = new("cursor-pointer");

    public static TailwindClass GridCols(string value) => new($"grid-cols-{value}");

    public static TailwindClass Text(ColorString color) => new($"text-[{color.Value}]");
    public static TailwindClass Bg(ColorString color) => new($"bg-[{color.Value}]");
    public static TailwindClass Border(ColorString color) => new($"border-[{color.Value}]");
    public static TailwindClass BorderY(string value) => new($"border-y-{value}");

    public static TailwindClass MaxW(string value) => new($"max-w-{value}");
    public static TailwindClass Gap(string value) => new($"gap-{value}");
    public static TailwindClass GapX(string value) => new($"gap-x-{value}");
    public static TailwindClass GapY(string value) => new($"gap-y-{value}");

    public static TailwindClass P(string value) => new($"p-{value}");
    public static TailwindClass Px(string value) => new($"px-{value}");
    public static TailwindClass Py(string value) => new($"py-{value}");

    public static TailwindClass Duration(string value) => new($"duration-{value}");

    public static TailwindClass Hover(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "hover:" + x.Value)));

    public static TailwindClass Focus(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "focus:" + x.Value)));

    public static TailwindClass Placeholder(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "placeholder:" + x.Value)));

    public readonly record struct TailwindClass(string Value);

    public static string ClassString(params IEnumerable<TailwindClass> tailwindClasses)
    {
        var s = string.Join(" ", tailwindClasses.Select(tc => tc.Value));
        return s;
    }
}