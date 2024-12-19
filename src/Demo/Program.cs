using System.Diagnostics.CodeAnalysis;
using System.Text;
using Demo.Commands;
using Spectre.Console.Cli;

namespace Demo;

public class Program
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(RunCommand))]
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(PackCommand))]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.All,
        "Olve.Grids.IO.Readers.TileAtlasAdjacenciesFileReader",
        "Olve.Grids.IO"
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.All,
        "Spectre.Console.Cli.ExplainCommand",
        "Spectre.Console.Cli"
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.All,
        "Spectre.Console.Cli.VersionCommand",
        "Spectre.Console.Cli"
    )]
    [DynamicDependency(
        DynamicallyAccessedMemberTypes.All,
        "Spectre.Console.Cli.XmlDocCommand",
        "Spectre.Console.Cli"
    )]
    public static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.Unicode;

        var commandApp = new CommandApp();

        commandApp.Configure(c =>
        {
            c.AddBranch(RunCommand.Name,
                y =>
                {
                    y.SetDefaultCommand<RunCommand>();
                    y.AddCommand<RunPackedCommand>(RunPackedCommand.Name);
                });

            c.AddCommand<PackCommand>(PackCommand.Name);
        });

        return await commandApp.RunAsync(args);
    }
}