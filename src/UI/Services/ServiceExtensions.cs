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
        services.AddTransient<LoadProjectOperation>();
        services.AddTransient<LoadProjectSummaryOperation>();
        services.AddTransient<DeleteProjectOperation>();
        services.AddTransient<CreateNewProjectOperation>();

        services.AddSingleton<IProjectSearchingRepository, FileBasedProjectSearchingRepository>();
        services.AddSingleton<IProjectSettingRepository, FileBasedProjectSettingRepository>();
        services.AddSingleton<IProjectGettingRepository, FileBasedProjectGettingRepository>();
        services.AddSingleton<IProjectDeletionRepository, FileBasedProjectDeletionRepository>();

        services.AddSingleton<LoggingService>();
    }
}