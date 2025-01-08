using System.ComponentModel;
using Olve.Grids.DeBroglie;
using Olve.Grids.Generation;
using Olve.Grids.IO;
using Olve.Grids.IO.Readers;
using Olve.Grids.Serialization;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo.Commands;

public class RunPackedSettings : CommandSettings
{
    [Description("The tile atlas file. Defaults to 'assets/tile-atlas.png'")]
    [CommandOption("-a|--tile-atlas")]
    public string TileAtlasFile { get; set; } = string.Empty;

    [Description("The packed tile atlas file ending in .grids.")]
    [CommandArgument(0, "<packed-tile-atlas>")]
    public string PackedTileAtlasFile { get; set; } = string.Empty;

    [Description("The input file containing brushes to generate. Defaults to 'assets/input.brushes.txt'")]
    [CommandOption("-i|--input")]
    public string InputBrushesFile { get; set; } = "assets/input.brushes.txt";

    [Description("The output file to generate. Defaults to 'output.png'")]
    [CommandOption("-o|--output")]
    public string OutputFile { get; set; } = "output.png";

    [Description("The verbosity level. Defaults to 'Normal'. Available values are 'Quiet', 'Normal', and 'Verbose'.")]
    [CommandOption("-v|--verbosity")]
    public string Verbosity { get; set; } = "Normal";

    [Description("Overwrite the output file if it already exists.")]
    [CommandOption("--overwrite")]
    public bool Overwrite { get; set; }
}

public class RunPackedCommand : Command<RunPackedSettings>
{
    public const string Name = "packed";

    private const int Success = 0;
    private const int Error = 1;

    public override int Execute(CommandContext context, RunPackedSettings settings)
    {
        var tileAtlasSerializer = new TileAtlasSerializer();
        var tileAtlasFileLoader = new TileAtlasFileLoader(tileAtlasSerializer);

        if (!tileAtlasFileLoader
                .Load(settings.PackedTileAtlasFile)
                .TryPickT0(out var tileAtlas, out var fileLoadingError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {fileLoadingError}");
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

        var imageLoader = new ImageLoader();
        if (!imageLoader
                .LoadImage(settings.TileAtlasFile)
                .TryPickValue(out var tileAtlasImage, out var imageLoaderError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {imageLoaderError}");
            return Error;
        }

        AnsiConsole.MarkupLine("[bold yellow]Packed tile atlas information:[/]");

        AnsiConsole.MarkupLine($"Tile size: [bold]{tileAtlas.Grid.TileSize.Width}x{tileAtlas.Grid.TileSize.Height}[/]");
        AnsiConsole.MarkupLine($"Tile count: [bold]{tileAtlas.Grid.Columns * tileAtlas.Grid.Rows}[/]");
        AnsiConsole.MarkupLine($"Brush count: [bold]{tileAtlas.BrushLookup.Brushes.Count()}[/]");

        var generator = new DeBroglieGenerator();

        var generationRequest = new GenerationRequest(tileAtlas, brushGrid);
        var generationResult = generator.Execute(generationRequest);

        var visualizationExporter = new VisualizationExporter();
        visualizationExporter.ExportAsPng(generationResult, settings.OutputFile, tileAtlasImage);

        if (generationResult.Status.IsT2)
        {
            return Error;
        }

        return Success;
    }
}