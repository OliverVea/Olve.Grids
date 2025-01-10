using Microsoft.Extensions.DependencyInjection;
using Olve.Utilities.AsyncOnStartup;
using UI.Core.Logging;
using UI.Core.Services.Brushes;
using UI.Core.Services.Projects;
using UI.Core.Services.Projects.FileSystem;
using UI.Core.Services.Projects.Repositories;
using UI.Core.Services.Tiles;

namespace UI.Core;

public static class ServiceExtensions
{
    public static void AddServices(this IServiceCollection services)
    {
        // Projects
        services.AddTransient<ListProjectSummariesOperation>();
        services.AddTransient<LoadProjectOperation>();
        services.AddTransient<LoadProjectSummaryOperation>();
        services.AddTransient<DeleteProjectOperation>();
        services.AddTransient<CreateNewProjectOperation>();
        services.AddTransient<SaveProjectAndSummaryOperation>();
        services.AddTransient<UpdateCurrentProjectOperation>();
        services.AddTransient<GetCurrentProjectOperation>();
        services.AddTransient<SetCurrentProjectOperation>();

        // Tiles
        services.AddTransient<ActivateTileOperation>();
        services.AddTransient<DeactivateTileOperation>();

        // Brushes
        services.AddTransient<CreateNewBrushOperation>();
        services.AddTransient<ListBrushesOperation>();
        services.AddTransient<SetBrushForTileCornerOperation>();

        services.AddSingleton<ICurrentProjectRepository, InMemoryCurrentProjectRepository>();

        services.AddTransient<IProjectSearchingRepository, FileBasedProjectSearchingRepository>();
        services.AddTransient<IProjectSettingRepository, FileBasedProjectSettingRepository>();
        services.AddTransient<IProjectGettingRepository, FileBasedProjectGettingRepository>();
        services.AddTransient<IProjectDeletionRepository, FileBasedProjectDeletionRepository>();

        // Logging
        services.AddTransient<LoggingService>();
        services.AddTransient<ILoggingProvider, ConsoleLoggingProvider>();

        services.AddTransient<OperationFactory>();
        services.AddTransient<AsyncOperationFactory>();

        services.AddTransient<IAsyncOnStartup, RegisterSaveProjectOnProjectChangeStartup>();
    }
}