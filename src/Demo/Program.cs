using System.Diagnostics.CodeAnalysis;
using System.Text;
using Spectre.Console.Cli;

namespace Demo;

public class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(RunCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.ExplainCommand", "Spectre.Console.Cli")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.VersionCommand", "Spectre.Console.Cli")]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, "Spectre.Console.Cli.XmlDocCommand", "Spectre.Console.Cli")]
    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;
        
        var commandApp = new CommandApp();
        
        commandApp.SetDefaultCommand<RunCommand>();
        
        return await commandApp.RunAsync(args);
    }
}