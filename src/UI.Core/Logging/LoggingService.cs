using Olve.Utilities.Types.Results;

namespace UI.Core.Logging;

public class LoggingService(IEnumerable<ILoggingProvider> loggingProviders)
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
        var message = new LogMessage(problem.Message, problem.Args)
        {
            Exception = problem.Exception,
            Level = LogLevel.Error,
            Source = problem.Source,
            Tags = problem.Tags,
        };

        Log(message);
    }

    public void Log(LogMessage message)
    {
        foreach (var provider in loggingProviders)
        {
            provider.Log(message);
        }
    }
}