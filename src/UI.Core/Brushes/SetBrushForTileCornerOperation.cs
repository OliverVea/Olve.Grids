using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Brushes;

public class SetBrushForTileCornerOperation(UpdateCurrentProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<SetBrushForTileCornerOperation.Request>
{
    public record Request(TileIndex TileIndex, Corner Corner, BrushId BrushId);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        UpdateCurrentProjectOperation.Request updateRequest = new(p => SetBrushForTileCorner(request, p));
        var updateResult = await updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
        if (updateResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }

    private static Result SetBrushForTileCorner(Request request, Project project)
    {
        var brushLookup = project.TileAtlasBuilder.Configuration.BrushLookup;
        if (brushLookup is null)
        {
            return new ResultProblem("No brush lookup is available.");
        }

        brushLookup.SetCornerBrush(request.TileIndex, request.Corner, request.BrushId);

        return Result.Success();
    }
}