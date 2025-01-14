﻿using Microsoft.Extensions.DependencyInjection;
using Olve.Utilities.AsyncOnStartup;
using UI.Core.Logging;
using UI.Core.Brushes;
using UI.Core.Projects;
using UI.Core.Projects.FileSystem;
using UI.Core.Projects.Operations;
using UI.Core.Projects.Repositories;
using UI.Core.Tiles;

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
        services.AddTransient<GetBrushOperation>();
        services.AddTransient<CreateNewBrushOperation>();
        services.AddTransient<UpdateBrushOperation>();
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