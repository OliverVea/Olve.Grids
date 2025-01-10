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
        var message = new LogMessage(
            problem.Exception,
            LogLevel.Error,
            problem.Message,
            problem.Tags,
            problem.Source,
            problem.Args);

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