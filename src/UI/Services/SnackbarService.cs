using Avalonia.Logging;

namespace UI.Services;

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

        Logger.Sink?.Log(logLevel, string.Empty, problem.Source, problem.Message, problem.Args);
    }
}