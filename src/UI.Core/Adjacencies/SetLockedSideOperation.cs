using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Utilities.Operations;
using UI.Core.Projects;
using UI.Core.Projects.Operations;

namespace UI.Core.Adjacencies;

public class SetLockedSideOperation(UpdateProjectOperation updateProjectOperation)
    : IAsyncOperation<SetLockedSideOperation.Request>
{
    public record Request(Id<Project> ProjectId, TileIndex TileIndex, Side Side, bool IsLocked);


    public Task<Result> ExecuteAsync(Request request, CancellationToken ct = default)
    {
        UpdateProjectOperation.Request updateRequest = new(request.ProjectId, p => SetLockedSideInProject(p, request));
        return updateProjectOperation.ExecuteAsync(updateRequest, ct);
    }

    private static void SetLockedSideInProject(Project project, Request request)
    {
        var projectContains = project.LockedSides.Contains((request.TileIndex, request.Side));

        if (!projectContains && request.IsLocked)
        {
            project.LockedSides.Add((request.TileIndex, request.Side));
        }

        else if (projectContains && !request.IsLocked)
        {
            project.LockedSides.Remove((request.TileIndex, request.Side));
        }
    }
}