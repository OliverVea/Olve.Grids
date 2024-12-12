using System.ComponentModel;
using System.Diagnostics;
using Olve.Grids.Adjacencies;
using Olve.Grids.DeBroglie;
using Olve.Grids.FileIO;
using Olve.Grids.Generation;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo;

public class RunCommand : Command<RunCommand.Settings>
{
    private const int Quiet = 0;
    private const int Normal = 1;
    private const int Verbose = 2;
    
    public class Settings : CommandSettings
    {
        [Description("The tile atlas file. Defaults to 'tile-atlas.png'")]
        [CommandOption("-a|--tile-atlas")]
        public string TileAtlasFile { get; set; } = "tile-atlas.png";
        
        [Description("The tile size in pixels. Defaults to '4x4'")]
        [CommandOption("-s|--tile-size")]
        public string TileSize { get; set; } = "4x4";
        
        [Description("The tile atlas brushes file containing brushes for each tile. Defaults to 'tile-atlas.brushes.txt'")]
        [CommandOption("-b|--tile-atlas-brushes")]
        public string TileAtlasBrushesFile { get; set; } = "tile-atlas.brushes.txt";
        
        [Description("The input file containing brushes to generate. Defaults to 'input.brushes.txt'")]
        [CommandOption("-i|--input")]
        public string InputBrushesFile { get; set; } = "input.brushes.txt";
        
        [Description("The output file to generate. Defaults to 'output.png'")]
        [CommandOption("-o|--output")]
        public string OutputFile { get; set; } = "output.png";

        [Description("The verbosity level. Defaults to 'Normal'. Available values are 'Quiet', 'Normal', and 'Verbose'.")]
        [CommandOption("-v|--verbosity")]
        public string Verbosity { get; set; } = "Normal";
        
        [Description("Overwrite the output file if it already exists.")]
        [CommandOption("--overwrite")]
        public bool Overwrite { get; set; }

        public Olve.Utilities.IntegerMath2D.Size _tileSize;
        public int _verbosity;
    }

    public override ValidationResult Validate(CommandContext context, Settings settings)
    {
        if (string.IsNullOrWhiteSpace(settings.TileAtlasFile))
        {
            return ValidationResult.Error("The tile atlas file must be specified.");
        }
        
        if (!settings.TileAtlasFile.EndsWith(".png", StringComparison.InvariantCulture))
        {
            return ValidationResult.Error("The tile atlas file must be a PNG file.");
        }
        
        if (!File.Exists(settings.TileAtlasFile))
        {
            return ValidationResult.Error("The tile atlas file does not exist.");
        }
        
        if (string.IsNullOrWhiteSpace(settings.TileSize))
        {
            return ValidationResult.Error("The tile size must be specified.");
        }
        
        var tileSizeParts = settings.TileSize.Split('x');
        if (tileSizeParts.Length != 2)
        {
            return ValidationResult.Error("The tile size must be in the format '[width]x[height]', e.g. 4x4.");
        }
        
        if (!int.TryParse(tileSizeParts[0], out var width) || width <= 0)
        {
            return ValidationResult.Error("The tile width must be a positive integer.");
        }
        
        if (!int.TryParse(tileSizeParts[1], out var height) || height <= 0)
        {
            return ValidationResult.Error("The tile height must be a positive integer.");
        }
        
        settings._tileSize = new Olve.Utilities.IntegerMath2D.Size(width, height);
        
        if (string.IsNullOrWhiteSpace(settings.TileAtlasBrushesFile))
        {
            return ValidationResult.Error("The tile atlas brushes file must be specified.");
        }
        
        if (!File.Exists(settings.TileAtlasBrushesFile))
        {
            return ValidationResult.Error("The tile atlas brushes file does not exist.");
        }
        
        if (string.IsNullOrWhiteSpace(settings.InputBrushesFile))
        {
            return ValidationResult.Error("The input brushes file must be specified.");
        }
        
        if (!File.Exists(settings.InputBrushesFile))
        {
            return ValidationResult.Error("The input brushes file does not exist.");
        }
        
        if (string.IsNullOrWhiteSpace(settings.OutputFile))
        {
            return ValidationResult.Error("The output file must be specified.");
        }
        
        if (!settings.OutputFile.EndsWith(".png", StringComparison.InvariantCulture))
        {
            return ValidationResult.Error("The output file must be a PNG file.");
        }
        
        if (File.Exists(settings.OutputFile) && !settings.Overwrite)
        {
            return ValidationResult.Error("The output file already exists. Use the '--overwrite' option to overwrite it.");
        }
        
        if (string.IsNullOrWhiteSpace(settings.Verbosity))
        {
            return ValidationResult.Error("The verbosity level must be specified.");
        }
        
        settings.Verbosity = settings.Verbosity.ToLowerInvariant();
        
        if (settings.Verbosity is "quiet" or "q")
        {
            settings._verbosity = Quiet;
        }
        else if (settings.Verbosity is "normal" or "n")
        {
            settings._verbosity = Normal;
        }
        else if (settings.Verbosity is "verbose" or "v")
        {
            settings._verbosity = Verbose;
        }
        else
        {
            return ValidationResult.Error("The verbosity level must be 'Quiet', 'Normal', or 'Verbose' or 'q', 'n', or 'v'.");
        }
        
        return base.Validate(context, settings);
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        if (settings._verbosity > Quiet)
        {
            AnsiConsole.MarkupLine("[bold yellow]Running DeBroglie demo...[/]");
            AnsiConsole.MarkupLine($"Tile atlas file: [bold]{settings.TileAtlasFile}[/]");
            AnsiConsole.MarkupLine($"Tile size: [bold]{settings._tileSize.Width}x{settings._tileSize.Height}[/]");
            AnsiConsole.MarkupLine($"Tile atlas brushes file: [bold]{settings.TileAtlasBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Input brushes file: [bold]{settings.InputBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Output file: [bold]{settings.OutputFile}[/]");
        }
        
        var totalStart = Stopwatch.GetTimestamp();
        
        var tileAtlasBuilderStart = Stopwatch.GetTimestamp();
        var tileAtlasBuilder = TileAtlasBuilder
            .Create(settings.TileAtlasFile)
            .WithTileSize(settings._tileSize)
            .ReadBrushLookupFromFile(settings.TileAtlasBrushesFile);
    
        var brushGrid = tileAtlasBuilder.ReadBrushGridFromFile(settings.InputBrushesFile);
        var tileAtlasBuilderTime = Stopwatch.GetElapsedTime(tileAtlasBuilderStart);
        
        if (settings._verbosity == Verbose) AnsiConsole.MarkupLine($"Created tile atlas builder in: [bold]{tileAtlasBuilderTime}[/]");
        
        var tileAtlasStart = Stopwatch.GetTimestamp();
        var tileAtlas = tileAtlasBuilder.Build();
        var tileAtlasTime = Stopwatch.GetElapsedTime(tileAtlasStart);
        
        if (settings._verbosity == Verbose) AnsiConsole.MarkupLine($"Built tile atlas in: [bold]{tileAtlasTime}[/]");

        var adjacencyEstimator = new AdjacencyFromTileBrushEstimator();
        adjacencyEstimator.SetAdjacencies(tileAtlas.AdjacencyLookup, tileAtlas.BrushLookup);

        var request = new GenerationRequest(tileAtlas, brushGrid);
        var operation = new DeBroglieGenerator();

        var generationStart = Stopwatch.GetTimestamp();
        var result = operation.Execute(request);
        var generationTime = Stopwatch.GetElapsedTime(generationStart);
        
        if (settings._verbosity == Verbose) AnsiConsole.MarkupLine($"Generated output in: [bold]{generationTime}[/]");

        var visualizationExporter = new VisualizationExporter();
        visualizationExporter.ExportAsPng(result, settings.OutputFile);
        
        var totalEnd = Stopwatch.GetElapsedTime(totalStart);

        if (settings._verbosity > Quiet)
        {
            AnsiConsole.MarkupLine("[bold green]Done![/]");
            AnsiConsole.MarkupLine($"Wrote output to: [bold]{new FileInfo(settings.OutputFile).FullName}[/]");
        }
        
        if (settings._verbosity == Verbose)
        {
            AnsiConsole.MarkupLine($"Total time: [bold]{totalEnd}[/]");
        }

        return 0;
    }
}