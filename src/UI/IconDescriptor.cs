using Blazorise;

namespace UI;

public class IconDescriptor
{
    public string Class { get; }

    private IconDescriptor(string @class)
    {
        Class = @class;
    }

    public static readonly IconDescriptor Search = new("fa fa-search");
    public static readonly IconDescriptor ChevronLeft = new("fa fa-chevron-left");
    public static readonly IconDescriptor ChevronRight = new("fa fa-chevron-right");
    public static readonly IconDescriptor HorizontalRule = new("fa fa-horizontal-rule");
    public static readonly IconDescriptor VerticalRule = new("fa fa-vertical-rule");
}