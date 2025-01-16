using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Tiles;

public class ActivateTileOperation(UpdateProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<ActivateTileOperation.Request>
{
    public record Request(Id<Project> ProjectId, TileIndex TileIndex);

    public Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        UpdateProjectOperation.Request updateRequest = new(
            request.ProjectId,
            p => ActivateTile(p, request.TileIndex));

        return updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
    }

    private static void ActivateTile(Project project, TileIndex tileIndex)
    {
        project.ActiveTiles.Add(tileIndex);
    }
}