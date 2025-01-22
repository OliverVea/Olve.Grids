using Olve.Utilities.AsyncOnStartup;
using UI.Blazor.Components.ContextMenus;
using UI.Blazor.Components.Modals;
using UI.Blazor.Components.ProjectDashboard;
using UI.Blazor.Interop;
using UI.Blazor.Navigation;

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

        services.AddTransient<NavigationService>();

        services.AddTransient<IAsyncOnStartup, RegisterProjectDashboardStateChangeOnStartup>();
        services.AddTransient<ProjectDashboardStateHasChangedOperation>();
        services.AddSingleton<ProjectDashboardStateHasChangedOperation.Factory>();
        services.AddTransient<ProjectDashboardService>();
        services.AddSingleton<ProjectDashboardContainer>();


        return services;
    }
}