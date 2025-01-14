using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Services.Projects;

namespace UI.Core.Services.Brushes;

public class UpdateBrushOperation(UpdateCurrentProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<UpdateBrushOperation.Request>
{
    public record Request(BrushId BrushId, string? DisplayName, ColorString? Color);

    public record Response;


    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        UpdateCurrentProjectOperation.Request updateRequest = new(p => UpdateBrushInProject(p, request));
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