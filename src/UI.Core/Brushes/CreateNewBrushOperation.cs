using Olve.Grids.Brushes;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Brushes;

public class CreateNewBrushOperation(UpdateCurrentProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<CreateNewBrushOperation.Request, CreateNewBrushOperation.Response>
{
    public record Request(string? DisplayName = null);

    public record Response(ProjectBrush Brush);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        // Todo: Improve brush name logic
        //       Validate brush name:
        //       - Ensure it is not empty
        //       - Ensure it is not already in use
        //       If name not set, default to valid name, e.g. "New Brush", "New Brush 2", etc.

        var brush = NewBrush(request);

        UpdateCurrentProjectOperation.Request updateRequest = new(AddBrushToProject);
        var updateResult = await updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
        if (updateResult.TryPickProblems(out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(brush);

        Result AddBrushToProject(Project project) => project.Brushes.TryAdd(brush.Id, brush)
            ? Result.Success()
            : new ResultProblem("Brush with id already exists");
    }

    private static ProjectBrush NewBrush(Request request)
    {
        var brushId = GetNewBrushId();
        var brushName = request.DisplayName ?? "New Brush";
        var brushColor = GetInitialBrushColor(brushId);

        return new ProjectBrush(brushId, brushName, brushColor);
    }

    private static BrushId GetNewBrushId() => new(Id
        .NewId()
        .ToString());

    private static ColorString GetInitialBrushColor(BrushId brushId) =>
        CachedColorHelper.GetColorStringFromInteger(
            brushId.Value.GetHashCode(),
            HsvNormal.Default);
}