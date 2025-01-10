using Olve.Utilities.AsyncOnStartup;

namespace UI.Core.Services.Projects;

public class RegisterSaveProjectOnProjectChangeStartup(
    ICurrentProjectRepository currentProjectRepository,
    AsyncOperationFactory asyncOperationFactory) : IAsyncOnStartup
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
        SaveProjectAndSummaryOperation.Request request = new(project);
        var operation = asyncOperationFactory
            .Build<SaveProjectAndSummaryOperation, SaveProjectAndSummaryOperation.Request, Result>();

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