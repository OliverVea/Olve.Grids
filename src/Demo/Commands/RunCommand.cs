using System.Diagnostics;
using Olve.Grids.Adjacencies;
using Olve.Grids.DeBroglie;
using Olve.Grids.Generation;
using Olve.Grids.IO;
using Olve.Grids.IO.Configuration;
using Olve.Grids.IO.Readers;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Grids.Weights;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo.Commands;

public class RunCommand : Command<RunCommandSettings>
{
    public const string Name = "run";

    private const int Success = 0;
    private const int InvalidVerbosity = 1;
    private const int InvalidTileSize = 2;
    private const int TileAtlasBrushesError = 3;
    private const int AtlasConfigurationError = 4;
    private const int InputBrushesError = 5;
    private const int GenerationError = 6;

    public override int Execute(CommandContext context, RunCommandSettings settings)
    {
        if (!VerbosityLevels
                .Parse(settings.Verbosity)
                .TryPickT0(out var verbosityLevel, out var verbosityError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {verbosityError}");
            return InvalidVerbosity;
        }

        if (!SizeParser
                .Parse(settings.TileSize)
                .TryPickT0(out var tileSize, out var tileSizeErrorMessage))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {tileSizeErrorMessage}");
            return InvalidTileSize;
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

        var totalStart = Stopwatch.GetTimestamp();

        var tileAtlasBuilderStart = Stopwatch.GetTimestamp();

        var tileAtlasBuilder = new TileAtlasBuilder()
            .WithFilePath(settings.TileAtlasFile)
            .WithTileSize(tileSize);

        var tileAtlasBrushesReader = new TileAtlasBrushesFileReader(settings.TileAtlasBrushesFile);
        if (!tileAtlasBrushesReader
                .Load()
                .TryPickT0(out var tileAtlasBrushes, out var brushErrors))
        {
            foreach (var problem in brushErrors.Problems)
            {
                AnsiConsole.MarkupLine($"[bold red]Error:[/] {problem}");
            }

            return TileAtlasBrushesError;
        }

        tileAtlasBuilder.WithBrushLookupBuilder(tileAtlasBrushes);
        var tileIndices = tileAtlasBrushes
            .Entries
            .Select(x => x.TileIndex)
            .Distinct()
            .ToArray();

        if (settings.TileAtlasConfigFile is { } tileAtlasConfigFile)
        {
            var adjacencyBuilder = new AdjacencyLookup();
            var weightBuilder = new WeightLookup();

            var configurationLoader = ConfigurationLoader.Create();

            if (!configurationLoader
                    .Load(tileAtlasConfigFile, adjacencyBuilder, weightBuilder, tileIndices, tileAtlasBrushes.Entries)
                    .TryPickT0(out _, out var error))
            {
                foreach (var problem in error.Problems)
                {
                    AnsiConsole.MarkupLine($"[bold red]Error:[/] {problem}");
                }

                return AtlasConfigurationError;
            }

            tileAtlasBuilder = tileAtlasBuilder
                .WithAdjacencyLookupBuilder(adjacencyBuilder)
                .WithWeightLookupBuilder(weightBuilder);

            AnsiConsole.MarkupLine(
                $"Using tile atlas configuration file: [bold]{new FileInfo(tileAtlasConfigFile).FullName}[/]");
        }
        else
        {
            var builder = new AdjacencyLookup();

            var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
            adjacencyEstimator.SetAdjacencies(builder,
                tileAtlasBuilder.Configuration.BrushLookup?.Entries ?? throw new Exception());

            tileAtlasBuilder = tileAtlasBuilder.WithAdjacencyLookupBuilder(builder);

            AnsiConsole.MarkupLine(
                "[bold yellow]No tile atlas configuration file specified. Using default configuration...[/]");
        }

        var tileAtlasBuilderTime = Stopwatch.GetElapsedTime(tileAtlasBuilderStart);

        if (verbosityLevel.IsVerbose)
        {
            AnsiConsole.MarkupLine($"Created tile atlas builder in: [bold]{tileAtlasBuilderTime}[/]");
        }

        var tileAtlasStart = Stopwatch.GetTimestamp();
        var tileAtlas = tileAtlasBuilder.Build();

        var tileAtlasTime = Stopwatch.GetElapsedTime(tileAtlasStart);

        if (verbosityLevel.IsVerbose)
        {
            AnsiConsole.MarkupLine($"Built tile atlas in: [bold]{tileAtlasTime}[/]");
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

            return InputBrushesError;
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

        var totalEnd = Stopwatch.GetElapsedTime(totalStart);

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold green]Done![/]");
            AnsiConsole.MarkupLine($"Wrote output to: [bold]{new FileInfo(settings.OutputFile).FullName}[/]");
        }

        if (verbosityLevel.IsVerbose)
        {
            AnsiConsole.MarkupLine($"Total time: [bold]{totalEnd}[/]");
        }

        if (result.Status.IsT2)
        {
            return GenerationError;
        }

        return Success;
    }
}