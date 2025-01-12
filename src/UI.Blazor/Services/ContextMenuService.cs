using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using UI.Blazor.Components.Components;
using UI.Blazor.Components.Providers;
using UI.Blazor.Interop;

namespace UI.Blazor.Services;

public class ContextMenuService(ContextMenuProviderContainer providerContainer, ElementSizeInterop elementSizeInterop)
{
    private ContextMenuProvider Provider => providerContainer.Provider;

    public void Register(ContextMenuProvider provider) => providerContainer.SetProvider(provider);


    public ValueTask ShowAsync(MouseEventArgs e, RenderFragment childContent, string? title = null)
    {
        var position = new Position((int)e.ClientX, (int)e.ClientY);
        return ShowAsync(position, childContent, title);
    }

    public async ValueTask ShowAsync(Position position, RenderFragment childContent, string? title = null)
    {
        var menuPosition = await GetMenuPositionAsync(position);

        await Provider.MutateAsync(state =>
        {
            state.Title = title;
            state.ChildContent = childContent;
            state.Visible = true;
            state.Position = menuPosition;
        });
    }


    public async ValueTask HideIfPositionOutsideMenu(Position position)
    {
        var isOutside = await PositionOutsideContextMenu(position);

        if (isOutside)
        {
            await HideAsync();
        }
    }

    public async Task HideAsync()
    {
        await Provider.MutateAsync(state => state.Visible = false);
    }


    private async ValueTask<Position> GetMenuPositionAsync(Position newPosition)
    {
        var windowDimensions = await elementSizeInterop.GetWindowSize();
        var contextMenuSize = await GetMenuSizeAsync();

        var maxX = windowDimensions.Width - contextMenuSize.Width;
        var maxY = windowDimensions.Height - contextMenuSize.Height;

        var x = Math.Min(newPosition.X, maxX);
        var y = Math.Min(newPosition.Y, maxY);

        return new Position(x, y);
    }

    private async ValueTask<Size> GetMenuSizeAsync()
    {
        var size = await elementSizeInterop.GetElementSize(Provider.IdSelector);

        return size ?? new Size(0, 0);
    }

    private async ValueTask<bool> PositionOutsideContextMenu(Position position)
    {
        if (!Provider.Visible)
        {
            return true;
        }

        var dx = position.X - Provider.Position.X;
        var dy = position.Y - Provider.Position.Y;

        if (dx < 0 || dy < 0)
        {
            return true;
        }

        var contextMenuSize = await GetMenuSizeAsync();

        if (dx > contextMenuSize.Width || dy > contextMenuSize.Height)
        {
            return true;
        }

        return false;
    }
}