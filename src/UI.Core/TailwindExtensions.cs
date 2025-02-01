using System.Globalization;

namespace UI.Core;

public static class TailwindExtensions
{
    public static readonly TailwindClass Flex = new("flex");
    public static readonly TailwindClass Grow = new("grow");
    public static readonly TailwindClass FlexRow = new("flex-row");
    public static readonly TailwindClass FlexRowReverse = new("flex-row-reverse");
    public static readonly TailwindClass FlexCol = new("flex-col");
    public static readonly TailwindClass FlexColReverse = new("flex-col-reverse");

    public static readonly TailwindClass Hidden = new("hidden");

    public static readonly TailwindClass ResizeBoth = new("resize");
    public static readonly TailwindClass ResizeX = new("resize-x");
    public static readonly TailwindClass ResizeY = new("resize-y");

    public static readonly TailwindClass Absolute = new("absolute");
    public static readonly TailwindClass Relative = new("relative");
    public static readonly TailwindClass Fixed = new("fixed");

    public static readonly TailwindClass OverflowHidden = new("overflow-hidden");
    public static readonly TailwindClass OverflowAuto = new("overflow-auto");

    public static readonly TailwindClass OverflowXAuto = new("overflow-x-auto");
    public static readonly TailwindClass OverflowYAuto = new("overflow-y-auto");

    public static readonly TailwindClass OverflowXHidden = new("overflow-x-hidden");
    public static readonly TailwindClass OverflowYHidden = new("overflow-y-hidden");

    public static readonly TailwindClass border = new("border");
    public static readonly TailwindClass BorderL = new("border-l");
    public static readonly TailwindClass BorderR = new("border-r");
    public static readonly TailwindClass BorderT = new("border-t");
    public static readonly TailwindClass BorderB = new("border-b");
    public static readonly TailwindClass BorderSolid = new("border-solid");

    public static readonly TailwindClass ObjectContain = new("object-contain");

    public static readonly TailwindClass Grid = new("grid");

    public static readonly TailwindClass ItemsCenter = new("items-center");
    public static readonly TailwindClass JustifyCenter = new("justify-center");
    public static readonly TailwindClass JustifyBetween = new("justify-between");
    public static readonly TailwindClass JustifyStart = new("justify-start");
    public static readonly TailwindClass JustifyEnd = new("justify-end");

    public static readonly TailwindClass HFull = new("h-full");
    public static readonly TailwindClass WFull = new("w-full");
    public static readonly TailwindClass WScreen = new("w-screen");
    public static readonly TailwindClass HScreen = new("h-screen");
    public static readonly TailwindClass WAuto = new("w-auto");
    public static readonly TailwindClass HAuto = new("h-auto");
    public static readonly TailwindClass WFit = new("w-fit");
    public static readonly TailwindClass HFit = new("h-fit");

    public static readonly TailwindClass BorderNone = new("border-none");
    public static readonly TailwindClass Rounded = new("rounded");
    public static readonly TailwindClass RoundedMd = new("rounded-md");

    public static readonly TailwindClass TextNoWrap = new("text-no-wrap");
    public static readonly TailwindClass TextLeft = new("text-left");
    public static readonly TailwindClass TextRight = new("text-right");
    public static readonly TailwindClass TextCenter = new("text-center");

    public static readonly TailwindClass Shadow = new("shadow");
    public static readonly TailwindClass ShadowMd = new("shadow-md");
    public static readonly TailwindClass ShadowLg = new("shadow-lg");
    public static readonly TailwindClass ShadowXl = new("shadow-xl");


    public static readonly TailwindClass Underline = new("underline");

    public static readonly TailwindClass Transition = new("transition");

    public static readonly TailwindClass MinHScreen = new("min-h-screen");
    public static readonly TailwindClass MinWScreen = new("min-w-screen");
    public static readonly TailwindClass MaxHScreen = new("max-h-screen");
    public static readonly TailwindClass MaxWScreen = new("max-w-screen");
    
    public static readonly TailwindClass MaxWFull = new("max-w-full");
    public static readonly TailwindClass MaxHFull = new("max-h-full");
    public static readonly TailwindClass MinWFull = new("min-w-full");
    public static readonly TailwindClass MinHFull = new("min-h-full");

    public static readonly TailwindClass TextXs = new("text-xs");
    public static readonly TailwindClass TextSm = new("text-sm");
    public static readonly TailwindClass TextBase = new("text-base");
    public static readonly TailwindClass TextLg = new("text-lg");
    public static readonly TailwindClass TextXl = new("text-xl");
    public static readonly TailwindClass Text2Xl = new("text-2xl");
    public static readonly TailwindClass Text3Xl = new("text-3xl");
    public static readonly TailwindClass Text4Xl = new("text-4xl");
    public static readonly TailwindClass Text5Xl = new("text-5xl");
    public static readonly TailwindClass Text6Xl = new("text-6xl");
    public static readonly TailwindClass Text7Xl = new("text-7xl");
    public static readonly TailwindClass Text8Xl = new("text-8xl");

    public static readonly TailwindClass FontThin = new("font-thin");
    public static readonly TailwindClass FontLight = new("font-light");
    public static readonly TailwindClass FontNormal = new("font-normal");
    public static readonly TailwindClass FontMedium = new("font-medium");
    public static readonly TailwindClass FontSemibold = new("font-semibold");
    public static readonly TailwindClass FontBold = new("font-bold");

    public static readonly TailwindClass OutlineNone = new("outline-none");
    public static readonly TailwindClass ItemsStart = new("items-start");
    public static readonly TailwindClass BreakWords = new("break-words");

    public static readonly TailwindClass CursorPointer = new("cursor-pointer");
    public static readonly TailwindClass CursorDefault = new("cursor-default");
    public static readonly TailwindClass CursorText = new("cursor-text");
    public static readonly TailwindClass CursorNone = new("cursor-none");

    public static readonly TailwindClass MarginAuto = new("m-auto");

    public static TailwindClass RoundedFull = new("rounded-full");

    public static TailwindClass TransitionTransform = new("transition-transform");
    public static TailwindClass TransitionOpacity = new("transition-opacity");
    public static TailwindClass Transform => new("transform");


    public static TailwindClass Aspect(string value) => new($"aspect-[{value}]");
    public static TailwindClass Aspect(int a, int b) => new($"aspect-{a}/{b}");
    public static TailwindClass Basis(string value) => new($"basis-[{value}]");
    public static TailwindClass Basis(int a, int b) => new($"basis-{a}/{b}");

    public static TailwindClass GridCols(string value) => new($"grid-cols-[{value}]");
    public static TailwindClass GridCols(int value) => new($"grid-cols-{value}");
    public static TailwindClass GridRows(string value) => new($"grid-rows-[{value}]");
    public static TailwindClass GridRows(int value) => new($"grid-rows-{value}");
    public static TailwindClass ColSpan(int value) => new($"col-span-{value}");
    public static TailwindClass RowSpan(int value) => new($"row-span-{value}");

    public static TailwindClass Opacity(int value) => new($"opacity-{value}");
    public static TailwindClass Scale(int value) => new($"scale-{value}");

    public static TailwindClass Top(int value) => new($"top-{value}");
    public static TailwindClass Top(string value) => new($"top-[{value}]");
    public static TailwindClass Left(int value) => new($"left-{value}");
    public static TailwindClass Left(string value) => new($"left-[{value}]");
    public static TailwindClass Right(int value) => new($"right-{value}");
    public static TailwindClass Right(string value) => new($"right-[{value}]");
    public static TailwindClass Bottom(int value) => new($"bottom-{value}");
    public static TailwindClass Bottom(string value) => new($"bottom-[{value}]");
    public static TailwindClass Inset(int value) => new($"inset-{value}");

    public static TailwindClass Z(int value) => new($"z-{value}");
    public static TailwindClass H(int value) => new($"h-{value}");
    public static TailwindClass H(string value) => new($"h-[{value}]");
    public static TailwindClass W(int value) => new($"w-{value}");
    public static TailwindClass W(string value) => new($"w-[{value}]");

    public static Pixels px(int value) => new(value);
    public static Percent pct(float value) => new(value);

    public static TailwindClass Text(ColorString color) => new($"text-[{color.Value}]");

    public static TailwindClass Bg(ColorString color) => new($"bg-[{color.Value}]");
    public static TailwindClass Border(ColorString color) => new($"border-[{color.Value}]");
    public static TailwindClass Border(int value) => new($"border-{value}");
    public static TailwindClass BorderLeft(string value) => new($"border-l-[{value}]");
    public static TailwindClass BorderRight(string value) => new($"border-r-[{value}]");
    public static TailwindClass BorderTop(string value) => new($"border-t-[{value}]");
    public static TailwindClass BorderBottom(string value) => new($"border-b-[{value}]");

    public static TailwindClass BorderOpacity(int value) => new($"border-opacity-{value}");
    public static TailwindClass BorderY(int value) => new($"border-y-{value}");

    public static TailwindClass MaxW(string value) => new($"max-w-[{value}]");
    public static TailwindClass MaxW(int a, int b) => new($"max-w-{a}/{b}");
    public static TailwindClass MaxH(string value) => new($"max-h-[{value}]");
    public static TailwindClass MinW(string value) => new($"min-w-[{value}]");
    public static TailwindClass MinH(string value) => new($"min-h-[{value}]");
    public static TailwindClass Gap(int value) => new($"gap-{value}");
    public static TailwindClass GapX(int value) => new($"gap-x-{value}");
    public static TailwindClass GapY(int value) => new($"gap-y-{value}");

    public static TailwindClass P(int value) => new($"p-{value}");
    public static TailwindClass P(string value) => new($"p-[{value}]");
    public static TailwindClass Px(int value) => new($"px-{value}");
    public static TailwindClass Py(int value) => new($"py-{value}");
    public static TailwindClass Pl(int value) => new($"pl-{value}");
    public static TailwindClass Pr(int value) => new($"pr-{value}");
    public static TailwindClass Pt(int value) => new($"pt-{value}");
    public static TailwindClass Pt(string value) => new($"pt-[{value}]");
    public static TailwindClass Pb(int value) => new($"pb-{value}");

    public static TailwindClass M(int value) => new($"m-{value}");
    public static TailwindClass Mx(int value) => new($"mx-{value}");
    public static TailwindClass Mx(string value) => new($"mx-[{value}]");
    public static TailwindClass My(int value) => new($"my-{value}");
    public static TailwindClass My(string value) => new($"my-[{value}]");
    public static TailwindClass Ml(int value) => new($"ml-{value}");
    public static TailwindClass Mr(int value) => new($"mr-{value}");
    public static TailwindClass Mt(int value) => new($"mt-{value}");
    public static TailwindClass Mb(int value) => new($"mb-{value}");

    public static TailwindClass PointerEvents(string value) => new($"pointer-events-{value}");

    public static TailwindClass Duration(string value) => new($"duration-{value}");

    public static TailwindClass Hover(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "hover:" + x.Value)));

    public static TailwindClass Focus(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "focus:" + x.Value)));

    public static TailwindClass Placeholder(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "placeholder:" + x.Value)));

    public static TailwindClass File(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "file:" + x.Value)));

    public static TailwindClass Before(params IEnumerable<TailwindClass> tailwindClasses) =>
        new(string.Join(" ", tailwindClasses.Select(x => "before:" + x.Value)));

    public static string TW(params IEnumerable<TailwindClass> tailwindClasses)
    {
        var s = string.Join(" ", tailwindClasses.Select(tc => tc.Value));
        return s;
    }

    public readonly record struct Pixels(int Value)
    {
        public static implicit operator string(Pixels pixels) => $"{pixels.Value}px";
    }

    public readonly record struct Percent(float Value)
    {
        public static implicit operator string(Percent percent) =>
            $"{percent.Value.ToString(CultureInfo.InvariantCulture)}%";
    }

    public readonly record struct TailwindClass(string Value)
    {
        public override string ToString() => Value;
    }
}