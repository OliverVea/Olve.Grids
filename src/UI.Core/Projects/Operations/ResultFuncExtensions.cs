namespace UI.Core.Projects.Operations;

public static class ResultFuncExtensions
{
    public static Func<T, Result> ToFunc<T>(this Action<T> action) => t =>
    {
        action(t);
        return Result.Success();
    };
}