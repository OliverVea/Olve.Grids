using Microsoft.Extensions.DependencyInjection;
using UI.Services.Projects;
using UI.Services.Projects.FileSystem;

namespace UI.Services;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        services.AddTransient<ListProjectSummariesOperation>();
        services.AddTransient<CreateNewProjectOperation>();

        services.AddSingleton<IProjectRepository, FileBasedProjectRepository>();

        services.AddSingleton<LoggingService>();
    }
}