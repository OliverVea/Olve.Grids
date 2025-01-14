namespace UI.Core;

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
    public static readonly IconDescriptor ChevronUp = new("fa fa-chevron-up");
    public static readonly IconDescriptor ChevronDown = new("fa fa-chevron-down");
    public static readonly IconDescriptor HorizontalRule = new("fa fa-horizontal-rule");
    public static readonly IconDescriptor VerticalRule = new("fa fa-vertical-rule");
    public static readonly IconDescriptor Plus = new("fa fa-plus");
    public static readonly IconDescriptor Minus = new("fa fa-minus");
    public static readonly IconDescriptor Trash = new("fa fa-trash");
    public static readonly IconDescriptor Edit = new("fa fa-pencil");
    public static readonly IconDescriptor Check = new("fa fa-check");
    public static readonly IconDescriptor Circle = new("fa fa-circle");
    public static readonly IconDescriptor CircleSolid = new("fa-solid fa-circle");
    public static readonly IconDescriptor XSolid = new("fa-solid fa-xmark");
}