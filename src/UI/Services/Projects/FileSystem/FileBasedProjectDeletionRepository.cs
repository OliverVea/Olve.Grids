﻿using UI.Services.Projects.Repositories;

namespace UI.Services.Projects.FileSystem;

public class FileBasedProjectDeletionRepository(
    LoadProjectSummaryOperation loadProjectSummaryOperation,
    LoadProjectOperation loadProjectOperation) : IProjectDeletionRepository
{
    public async Task<Result> DeleteProjectAsync(Id<Project> projectId, CancellationToken ct = default)
    {
        LoadProjectOperation.Request loadProjectRequest = new(projectId);
        var loadProjectResult = await loadProjectOperation.ExecuteAsync(loadProjectRequest, ct);
        if (!loadProjectResult.TryPickValue(out var project, out var problems))
        {
            return Result.Failure(problems);
        }

        LoadProjectSummaryOperation.Request loadSummaryRequest = new(projectId);
        var loadSummaryResult = await loadProjectSummaryOperation.ExecuteAsync(loadSummaryRequest, ct);
        if (!loadSummaryResult.TryPickValue(out var projectSummary, out problems))
        {
            return Result.Failure(problems);
        }

        var projectResult = ProjectFileHelper.Delete(project.Project);
        if (projectResult.TryPickProblems(out problems))
        {
            return Result.Failure(problems);
        }

        var projectSummaryResult = ProjectSummaryFileHelper.Delete(projectSummary.ProjectSummary);
        if (projectSummaryResult.TryPickProblems(out problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }
}