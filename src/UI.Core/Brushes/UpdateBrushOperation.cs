using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Brushes;

public class UpdateBrushOperation(UpdateProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<UpdateBrushOperation.Request>
{
    public record Request(Id<Project> ProjectId, BrushId BrushId, string? DisplayName, ColorString? Color);

    public record Response;


    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        UpdateProjectOperation.Request updateRequest = new(request.ProjectId, p => UpdateBrushInProject(p, request));
        var updateResult = await updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
        if (updateResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        return Result.Success();
    }

    private static Result UpdateBrushInProject(Project project, Request request)
    {
        if (!project.Brushes.TryGetValue(request.BrushId, out var brush))
        {
            return new ResultProblem("Brush with id '{0}' not found.", request.BrushId);
        }

        var updatedBrush = brush with
        {
            DisplayName = request.DisplayName ?? brush.DisplayName,
            Color = request.Color ?? brush.Color,
        };

        project.Brushes[request.BrushId] = updatedBrush;

        return Result.Success();
    }
}