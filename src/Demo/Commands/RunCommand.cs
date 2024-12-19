using System.Diagnostics;
using Olve.Grids.DeBroglie;
using Olve.Grids.Generation;
using Olve.Grids.IO;
using Olve.Grids.IO.Readers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo.Commands;

public class RunCommand : Command<RunCommandSettings>
{
    public const string Name = "run";

    private const int Success = 0;
    private const int Error = 1;

    public override ValidationResult Validate(CommandContext context, RunCommandSettings settings)
    {
        var validator = new RunCommandValidator();

        var result = validator.Validate(settings);

        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] {error}");
            }

            return ValidationResult.Error("The input parameters are invalid.");
        }

        return ValidationResult.Success();
    }

    public override int Execute(CommandContext context, RunCommandSettings settings)
    {
        if (!VerbosityLevels
                .Parse(settings.Verbosity)
                .TryPickT0(out var verbosityLevel, out var verbosityError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {verbosityError}");
            return Error;
        }

        if (!SizeParser
                .Parse(settings.TileSize)
                .TryPickT0(out var tileSize, out var tileSizeErrorMessage))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {tileSizeErrorMessage}");
            return Error;
        }

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold yellow]Running DeBroglie demo...[/]");
            AnsiConsole.MarkupLine($"Tile atlas file: [bold]{settings.TileAtlasFile}[/]");
            AnsiConsole.MarkupLine(
                $"Tile size: [bold]{tileSize.Width}x{tileSize.Height}[/]");
            AnsiConsole.MarkupLine($"Tile atlas brushes file: [bold]{settings.TileAtlasBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Input brushes file: [bold]{settings.InputBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Output file: [bold]{settings.OutputFile}[/]");
        }

        var tileAtlasLoader = new TileAtlasLoader();
        if (!tileAtlasLoader
                .LoadTileAtlas(settings.TileAtlasFile,
                    tileSize,
                    settings.TileAtlasBrushesFile,
                    settings.TileAtlasConfigFile)
                .TryPickT0(out var tileAtlas, out var tileAtlasError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {tileAtlasError}");
            return Error;
        }

        var inputBrushFileReader = new InputBrushFileReader(settings.InputBrushesFile);
        if (!inputBrushFileReader
                .Load(tileAtlas.BrushLookup.Brushes)
                .TryPickT0(out var brushGrid, out var fileParsingError))
        {
            foreach (var problem in fileParsingError.Problems)
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] {problem}");
            }

            return Error;
        }

        var request = new GenerationRequest(tileAtlas, brushGrid);
        var operation = new DeBroglieGenerator();

        var generationStart = Stopwatch.GetTimestamp();
        var result = operation.Execute(request);
        var generationTime = Stopwatch.GetElapsedTime(generationStart);

        if (verbosityLevel.IsVerbose)
        {
            AnsiConsole.MarkupLine($"Generated output in: [bold]{generationTime}[/]");
        }

        var visualizationExporter = new VisualizationExporter();
        visualizationExporter.ExportAsPng(result, settings.OutputFile);

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold green]Done![/]");
            AnsiConsole.MarkupLine($"Wrote output to: [bold]{new FileInfo(settings.OutputFile).FullName}[/]");
        }

        if (result.Status.IsT2)
        {
            return Error;
        }

        return Success;
    }
}