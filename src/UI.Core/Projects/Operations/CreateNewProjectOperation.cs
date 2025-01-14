using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Utilities.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects.Operations;

public class CreateNewProjectOperation(
    IProjectSettingRepository projectSettingRepository,
    IProjectGettingRepository projectGettingRepository)
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

        var tileAtlasBuilder = new TileAtlasBuilder()
            .WithTileSize(request.TileSize)
            .WithImageSize(imageSize);

        var project = new Project(id,
            projectName,
            createdAt,
            lastAccessedAt,
            request.TileSheetImage,
            [ ],
            [ ],
            tileAtlasBuilder);

        var createResult = await projectSettingRepository.SetProjectAsync(project, ct);
        if (createResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        var projectPathResult = await projectGettingRepository.GetProjectPathAsync(id, ct);
        if (!projectPathResult.TryPickValue(out var projectPath, out problems))
        {
            return problems;
        }

        var projectSummary = new ProjectSummary(id, projectName, projectPath, lastAccessedAt);

        var summaryResult = await projectSettingRepository.SetProjectSummaryAsync(projectSummary, ct);
        if (summaryResult.TryPickProblems(out problems))
        {
            return problems;
        }

        return new Response(projectSummary);
    }
}