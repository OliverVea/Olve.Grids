﻿using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Brushes;

public class GetBrushOperation(GetProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<GetBrushOperation.Request, GetBrushOperation.Response>
{
    public record Request(Id<Project> ProjectId, BrushId BrushId);

    public record Response(ProjectBrush Brush);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetProjectOperation.Request currentProjectRequest = new(request.ProjectId);
        var projectResult = await getCurrentProjectOperation.ExecuteAsync(currentProjectRequest, ct);
        if (projectResult.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        var brushes = project.Brushes;
        if (brushes.TryGetValue(request.BrushId, out var brush))
        {
            return new Response(brush);
        }

        return new ResultProblem("Brush with id '{0}' was not found in project", request.BrushId);
    }
}