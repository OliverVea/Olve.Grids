using Olve.Utilities.AsyncOnStartup;
using UI.Core.Projects.Operations;
using UI.Core.Projects.Repositories;

namespace UI.Core.Projects;

public class RegisterSaveProjectOnProjectChangeStartup(
    ICurrentProjectRepository currentProjectRepository,
    SaveProjectAndSummaryOperation.Factory operationFactory) : IAsyncOnStartup
{

    public Task OnStartupAsync(CancellationToken cancellationToken = new())
    {
        OnStartup();
        return Task.CompletedTask;
    }

    private void OnStartup()
    {
        currentProjectRepository.OnCurrentProjectChanged.Add(SaveProjectAndSummary);
    }

    private async Task SaveProjectAndSummary(Project project)
    {
        var operation = operationFactory.Build();

        SaveProjectAndSummaryOperation.Request request = new(project);
        var result = await operation.ExecuteAsync(request);

        if (result.TryPickProblems(out var problems))
        {
            foreach (var problem in problems)
            {
                Console.WriteLine(problem.Message, problem.Args);
            }
        }
    }
}