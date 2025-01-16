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
        var result = ExecuteInternal(settings);
        if (result.TryPickProblems(out var problems))
        {
            problems.LogToAnsiConsole();
            return Error;
        }

        return Success;
    }

    private static Result ExecuteInternal(RunCommandSettings settings)
    {
        var verbosityLevelResult = VerbosityLevels.Parse(settings.Verbosity);
        if (verbosityLevelResult.TryPickProblems(out var problems, out var verbosityLevel))
        {
            return problems;
        }

        var sizeResult = SizeParser.Parse(settings.TileSize);
        if (sizeResult.TryPickProblems(out problems, out var tileSize))
        {
            return problems;
        }

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold yellow]Running DeBroglie demo...[/]");
            AnsiConsole.MarkupLine($"Tile atlas file: [bold]{settings.TileAtlasFile}[/]");
            AnsiConsole.MarkupLine($"Tile size: [bold]{tileSize.Width}x{tileSize.Height}[/]");
            AnsiConsole.MarkupLine($"Tile atlas brushes file: [bold]{settings.TileAtlasBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Input brushes file: [bold]{settings.InputBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Output file: [bold]{settings.OutputFile}[/]");
        }

        var imageResult = new ImageLoader().LoadImage(settings.TileAtlasFile);
        if (imageResult.TryPickProblems(out problems, out var tileAtlasImage))
        {
            return problems;
        }

        var tileAtlasSize = new Size(tileAtlasImage.Width, tileAtlasImage.Height);

        var tileAtlasResult = new TileAtlasLoader().LoadTileAtlas(
            tileAtlasSize,
            tileSize,
            settings.TileAtlasBrushesFile,
            settings.TileAtlasConfigFile);
        if (tileAtlasResult.TryPickProblems(out problems, out var tileAtlas))
        {
            return problems;
        }

        var brushGridResult = new InputBrushFileReader(settings.InputBrushesFile).Load();
        if (brushGridResult.TryPickProblems(out problems, out var brushGrid))
        {
            return problems;
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
        visualizationExporter.ExportAsPng(result, settings.OutputFile, tileAtlasImage);

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold green]Done![/]");
            AnsiConsole.MarkupLine($"Wrote output to: [bold]{new FileInfo(settings.OutputFile).FullName}[/]");
        }

        if (result.Result.TryPickProblems(out problems))
        {
            return problems;
        }

        return Result.Success();
    }
}