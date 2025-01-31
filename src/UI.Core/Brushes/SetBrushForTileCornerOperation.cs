using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Brushes;

public class SetBrushForTileCornerOperation(
    UpdateProjectOperation updateCurrentProjectOperation,
    EstimateAdjacenciesFromBrushesCommand estimateAdjacenciesFromBrushesCommand)
    : IAsyncOperation<SetBrushForTileCornerOperation.Request>
{
    public record Request(Id<Project> ProjectId, TileIndex TileIndex, Corner Corner, BrushIdOrAny BrushId);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        UpdateProjectOperation.Request updateRequest = new(request.ProjectId, p => SetBrushForTileCorner(request, p));
        var updateResult = await updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
        if (updateResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }

    private Result SetBrushForTileCorner(Request request, Project project)
    {
        project.BrushLookup.SetCornerBrush(request.TileIndex, request.Corner, request.BrushId);

        var toUpdate = Sides.All.Select(x => (request.TileIndex, x));

        EstimateAdjacenciesFromBrushesCommand.Request estimateRequest = new(project.AdjacencyLookup,
            project.BrushLookup.TileBrushes)
        {
            ToNotUpdate = project.LockedSides,
            ToUpdate = toUpdate,
        };

        return estimateAdjacenciesFromBrushesCommand.Execute(estimateRequest);
    }
}