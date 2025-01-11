using UI.Blazor.Interop;
using UI.Blazor.Services;

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

        return services;
    }
}