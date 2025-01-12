using UI.Blazor.Components.Brushes;
using UI.Blazor.Components.Providers;
using UI.Blazor.Interop;

namespace UI.Blazor;

public static class ServiceExtensions
{
    public static IServiceCollection AddBlazorServices(this IServiceCollection services)
    {
        services.AddTransient<ElementSizeInterop>();

        services.AddTransient<ContextMenuService>();
        services.AddSingleton<ContextMenuProviderContainer>();

        services.AddTransient<ModalService>();
        services.AddSingleton<ModalProviderContainer>();

        services.AddTransient<BrushColorService>();

        return services;
    }
}