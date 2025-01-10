using System.Diagnostics.CodeAnalysis;
using Microsoft.JSInterop;

namespace UI.Blazor.Interop;

public class ElementSizeInterop(IJSRuntime jsRuntime)
{
    private const string GetWindowSizeJSFunctionName = "getWindowSize";
    private const string GetElementSizeJSFunctionName = "getElementSize";

    public async Task<Size> GetWindowSize()
    {
        var jsSize = await jsRuntime.InvokeAsync<JsSize>(GetWindowSizeJSFunctionName);

        if (!TryMapSize(jsSize, out var size))
        {
            throw new InvalidOperationException("Failed to get window size");
        }

        return size.Value;
    }

    public async Task<Size?> GetElementSize(IElementSelector selector)
    {
        var jsSize = await jsRuntime.InvokeAsync<JsSize?>(GetElementSizeJSFunctionName, selector.Selector);

        TryMapSize(jsSize, out var size);

        return size;
    }

    private static bool TryMapSize(JsSize? jsSize, [NotNullWhen(true)] out Size? size)
    {
        if (jsSize is null)
        {
            size = null;
            return false;
        }

        size = new Size((int)jsSize.Width, (int)jsSize.Height);
        return true;
    }

    private class JsSize
    {
        public float Width { get; set; }
        public float Height { get; set; }
    }
}