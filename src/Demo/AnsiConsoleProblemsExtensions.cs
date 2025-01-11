using Spectre.Console;

namespace Demo;

public static class AnsiConsoleProblemsExtensions
{
    public static void LogToAnsiConsole(this ResultProblemCollection problems)
    {
        foreach (var problem in problems)
        {
            // Todo: Improve problem formatting.
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {problem}");
        }
    }
}