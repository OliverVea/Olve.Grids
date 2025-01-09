using Avalonia;

namespace UI.Avalonia;

internal sealed class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        App.RunAvaloniaAppWithHosting(args, BuildAvaloniaApp);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}