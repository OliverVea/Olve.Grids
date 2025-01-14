namespace UI.Core.Services.Projects;

public class InMemoryCurrentProjectRepository : ICurrentProjectRepository
{
    private Project? _project;
    private Result<Project?> CurrentProject => _project;

    public InMemoryCurrentProjectRepository()
    {
        _onCurrentProjectChanged.Add(ListenerExample);
    }

    public ValueTask<Result<Project?>> GetCurrentProjectAsync(CancellationToken ct = new()) =>
        ValueTask.FromResult(CurrentProject);

    public ValueTask<Result> SetCurrentProjectAsync(Project? project, CancellationToken ct = new())
    {
        _project = project;

        return ValueTask.FromResult(Result.Success());
    }

    public async ValueTask<Result> UpdateCurrentProjectAsync(Func<Project, Result> update, CancellationToken ct = new())
    {
        if (_project is null)
        {
            return Result.Failure(new ResultProblem("No project is currently selected."));
        }

        var updateResult = update(_project);
        if (updateResult.TryPickProblems(out var problems))
        {
            return problems;
        }

        foreach (var listener in OnCurrentProjectChanged)
        {
            await listener(_project);
        }

        return Result.Success();
    }

    private static Task ListenerExample(Project project)
    {
        Console.WriteLine($"Current project changed to {project.Name}");

        return Task.CompletedTask;
    }

    private readonly HashSet<Func<Project, Task>> _onCurrentProjectChanged = [ ];
    public ISet<Func<Project, Task>> OnCurrentProjectChanged => _onCurrentProjectChanged;
}