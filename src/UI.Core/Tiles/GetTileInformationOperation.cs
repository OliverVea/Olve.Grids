using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Tiles;

public class GetTileInformationOperation(GetProjectOperation getCurrentProjectOperation)
    : IAsyncOperation<GetTileInformationOperation.Request,
        GetTileInformationOperation.Response>
{
    public record Request(Id<Project> ProjectId, TileIndex TileId);

    public record Response(TileInformation TileInformation);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        GetProjectOperation.Request getCurrentProjectRequest = new(request.ProjectId);
        var result = await getCurrentProjectOperation.ExecuteAsync(getCurrentProjectRequest, ct);
        if (result.TryPickProblems(out var problems, out var project))
        {
            return problems;
        }

        var tileInformationResult = TileInformationExtractionHelper.ExtractTileInformation(project, request.TileId);
        if (tileInformationResult.TryPickProblems(out problems, out var tileInformation))
        {
            return problems;
        }

        return new Response(tileInformation);
    }
}