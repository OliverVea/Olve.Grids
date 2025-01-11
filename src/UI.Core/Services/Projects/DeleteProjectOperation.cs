﻿using Olve.Utilities.Operations;
using UI.Core.Services.Projects.Repositories;

namespace UI.Core.Services.Projects;

public class DeleteProjectOperation(IProjectDeletionRepository projectDeletionRepository)
    : IAsyncOperation<DeleteProjectOperation.Request>
{
    public record Request(Id<Project> ProjectId);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var result = await projectDeletionRepository.DeleteProjectAsync(request.ProjectId, ct);
        if (result.TryPickProblems(out var problems))
        {
            return problems;
        }

        return Result.Success();
    }
}