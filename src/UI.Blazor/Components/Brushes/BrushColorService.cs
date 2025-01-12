using Olve.Grids.Brushes;
using UI.Core;

namespace UI.Blazor.Components.Brushes;

public class BrushColorService
{
    public ColorString GetBrushColor(BrushId brushId) =>
        CachedColorHelper.GetColorStringFromInteger(
            brushId.Value.GetHashCode(),
            HsvNormal.Default);

}