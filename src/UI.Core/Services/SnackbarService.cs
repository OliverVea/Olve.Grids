using Avalonia.Logging;

namespace UI.Core.Services;

public class LoggingService
{
    public void Show(IEnumerable<ResultProblem> problems)
    {
        foreach (var problem in problems)
        {
            Show(problem);
        }
    }

    public void Show(ResultProblem problem)
    {
        var logLevel = LogSeverities.ToAvaloniaLogEventLevel(problem.Severity);

        Console.WriteLine(problem.Message, problem.Args);
        Logger.Sink?.Log(logLevel, string.Empty, problem.Source, problem.Message, problem.Args);
    }
}