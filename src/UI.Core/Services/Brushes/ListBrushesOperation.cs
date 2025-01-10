using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.Operations;
using UI.Core.Services.Projects;

namespace UI.Core.Services.Brushes;

public class ListBrushesOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<ListBrushesOperation.Request, Result<ListBrushesOperation.Response>>
{
    public record Request;

    public record Response(IEnumerable<BrushId> Brushes);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var currentProjectResult = await currentProjectRepository.GetCurrentProjectAsync(ct);
        if (currentProjectResult.TryPickProblems(out var problems, out var project))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(project.Brushes);
    }
}

public class CreateNewBrushOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<CreateNewBrushOperation.Request, Result<CreateNewBrushOperation.Response>>
{
    public record Request(string Name);

    public record Response(BrushId BrushId);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var brushId = BrushId.New(request.Name);

        var updateResult = await currentProjectRepository.UpdateCurrentProjectAsync(AddBrushToProject, ct);
        if (updateResult.TryPickProblems(out var problems))
        {
            return Result<Response>.Failure(problems);
        }

        return new Response(brushId);

        void AddBrushToProject(Project project) => project.Brushes.Add(brushId);
    }
}

public class SetBrushForTileCornerOperation(ICurrentProjectRepository currentProjectRepository)
    : IAsyncOperation<SetBrushForTileCornerOperation.Request, Result>
{
    public record Request(TileIndex TileIndex, Corner Corner, BrushId BrushId);

    public async Task<Result> ExecuteAsync(Request request, CancellationToken ct = new())
    {
        var setBrushResult = Result.Success();

        var updateResult =
            await currentProjectRepository.UpdateCurrentProjectAsync(
                p => SetBrushForTileCorner(request, p, out setBrushResult),
                ct);

        if (setBrushResult.TryPickProblems(out var problems))
        {
            return Result.Failure(problems);
        }

        if (updateResult.TryPickProblems(out problems))
        {
            return Result.Failure(problems);
        }

        return Result.Success();
    }

    private static void SetBrushForTileCorner(Request request, Project project, out Result result)
    {
        var brushLookup = project.TileAtlasBuilder.Configuration.BrushLookup;
        if (brushLookup is null)
        {
            result = Result.Failure(new ResultProblem("No brush lookup is available."));
            return;
        }

        brushLookup.SetCornerBrush(request.TileIndex, request.Corner, request.BrushId);

        result = Result.Success();
    }
}