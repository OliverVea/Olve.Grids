using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using UI.Core.Projects.Operations;

namespace UI.Core.Tiles;

public class GetTileInformationOperation(GetCurrentProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<GetTileInformationOperation.Request,
        GetTileInformationOperation.Response>
{
    public record Request(TileIndex TileId);

    public record Response(TileInformation TileInformation);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetCurrentProjectOperation.Request getCurrentProjectRequest = new();
        var result = await getCurrentProjectOperation.ExecuteAsync(getCurrentProjectRequest, ct);
        if (result.TryPickProblems(out var problems, out var projectResponse))
        {
            return problems;
        }

        var project = projectResponse.Project;

        var tileInformationResult = TileInformationExtractionHelper.ExtractTileInformation(project, request.TileId);
        if (tileInformationResult.TryPickProblems(out problems, out var tileInformation))
        {
            return problems;
        }

        return new Response(tileInformation);
    }
}