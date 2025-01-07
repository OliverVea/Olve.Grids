using Microsoft.Extensions.DependencyInjection;
using UI.Services.Projects;
using UI.Services.Projects.FileSystem;
using UI.Services.Projects.Repositories;

namespace UI.Services;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ListProjectSummariesOperation>();
        services.AddTransient<CreateNewProjectOperation>();

        services.AddSingleton<IProjectSearchingRepository, FileBasedProjectSearchingRepository>();
        services.AddSingleton<IProjectSettingRepository, FileBasedProjectSettingRepository>();
        services.AddSingleton<IProjectGettingRepository, FileBasedProjectGettingRepository>();

        services.AddSingleton<LoggingService>();
    }
}