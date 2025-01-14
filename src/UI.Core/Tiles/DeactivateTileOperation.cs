using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Tiles;

public class DeactivateTileOperation(UpdateCurrentProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<DeactivateTileOperation.Request>
{
    public record Request(TileIndex TileIndex);

    public Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var updateRequest = new UpdateCurrentProjectOperation.Request(project =>
        {
            project.ActiveTiles.Remove(request.TileIndex);
        });

        return updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
    }
}