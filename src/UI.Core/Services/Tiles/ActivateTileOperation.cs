using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using UI.Core.Services.Projects;

namespace UI.Core.Services.Tiles;

public class ActivateTileOperation(UpdateCurrentProjectOperation updateCurrentProjectOperation)
    : IAsyncOperation<ActivateTileOperation.Request, Result>
{
    public record Request(TileIndex TileIndex);

    public Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var updateRequest = new UpdateCurrentProjectOperation.Request(project =>
        {
            project.ActiveTiles.Add(request.TileIndex);
        });

        return updateCurrentProjectOperation.ExecuteAsync(updateRequest, ct);
    }
}