using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;
using Olve.Utilities.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class CreateNewProjectOperation(IProjectSettingRepository projectSettingRepository)
    : IAsyncOperation<
        CreateNewProjectOperation.Request,
        CreateNewProjectOperation.Response>
{
    public record Request(string Name, FileContent TileSheetImage, Size TileSize);

    public record Response(ProjectSummary ProjectSummary);

    public async Task<Result<Response>> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        var id = Id<Project>.NewId();
        var projectName = new ProjectName(request.Name);
        var createdAt = DateTimeOffset.Now;
        var lastAccessedAt = createdAt;
        var imageSize = new Size(request.TileSheetImage.Image.Width, request.TileSheetImage.Image.Height);


        var gridConfiguration = GetGridConfiguration(request, imageSize);

        var project = new Project
        {
            Id = id,
            Name = projectName,
            CreatedAt = createdAt,
            LastChangedAt = lastAccessedAt,
            TileSheetImage = request.TileSheetImage,
            GridConfiguration = gridConfiguration,
            WeightLookup = new WeightLookup(),
            AdjacencyLookup = new AdjacencyLookup(),
            BrushLookup = new BrushLookup(),
            ActiveTiles = [ ],
            Brushes = [ ],
            LockedSides = [ ],
        };

        var createResult = await projectSettingRepository.SetProjectAsync(project, ct);
        if (createResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        var projectSummary = new ProjectSummary(id, projectName, lastAccessedAt);

        var summaryResult = await projectSettingRepository.SetProjectSummaryAsync(projectSummary, ct);
        if (summaryResult.TryPickProblems(out problems))
        {
            return problems;
        }

        return new Response(projectSummary);
    }

    private GridConfiguration GetGridConfiguration(Request request, Size imageSize)
    {
        var rows = imageSize.Height / request.TileSize.Height;
        var columns = imageSize.Width / request.TileSize.Width;

        return new GridConfiguration(request.TileSize, rows, columns);
    }
}