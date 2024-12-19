using System.ComponentModel;
using System.Runtime.InteropServices.JavaScript;
using Olve.Grids.Serialization;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo.Commands;

public class PackCommandSettings : CommandSettings
{
    [Description("The tile atlas file. Defaults to 'assets/tile-atlas.png'")]
    [CommandOption("-a|--tile-atlas")]
    public string TileAtlasFile { get; set; } = "assets/tile-atlas.png";

    [Description("The tile size in pixels. Defaults to '4x4'")]
    [CommandOption("-s|--tile-size")]
    public string TileSize { get; set; } = "4x4";

    [Description(
        "The tile atlas brushes file containing brushes for each tile. Defaults to 'assets/tile-atlas.brushes.txt'")]
    [CommandOption("-b|--tile-atlas-brushes")]
    public string TileAtlasBrushesFile { get; set; } = "assets/tile-atlas.brushes.txt";

    [Description("The output file to generate. Defaults to 'output.grids'")]
    [CommandOption("-o|--output")]
    public string OutputFile { get; set; } = "output.grids";

    [Description("The verbosity level. Defaults to 'Normal'. Available values are 'Quiet', 'Normal', and 'Verbose'.")]
    [CommandOption("-v|--verbosity")]
    public string Verbosity { get; set; } = "Normal";

    [Description("Overwrite the output file if it already exists.")]
    [CommandOption("--overwrite")]
    public bool Overwrite { get; set; }

    [Description]
    [CommandOption("-c|--tile-atlas-config")]
    public string? TileAtlasConfigFile { get; set; }
}

public class PackCommand : Command<PackCommandSettings>
{
    public const string Name = "pack";

    private const int Success = 0;
    private const int Error = 1;

    public override int Execute(CommandContext context, PackCommandSettings settings)
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

        var serializer = new TileAtlasSerializer();
        var tileAtlasFileLoader = new TileAtlasFileLoader(serializer);

        if (!tileAtlasFileLoader
                .Save(tileAtlas, settings.OutputFile, settings.Overwrite)
                .TryPickT0(out _, out var savingError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {savingError}");
            return Error;
        }

        if (verbosityLevel.IsNormal)
        {
            AnsiConsole.MarkupLine("[bold yellow]Packing tile atlas...[/]");
            AnsiConsole.MarkupLine($"Tile atlas file: [bold]{settings.TileAtlasFile}[/]");
            AnsiConsole.MarkupLine(
                $"Tile size: [bold]{tileSize.Width}x{tileSize.Height}[/]");
            AnsiConsole.MarkupLine($"Tile atlas brushes file: [bold]{settings.TileAtlasBrushesFile}[/]");
            AnsiConsole.MarkupLine($"Output file: [bold]{settings.OutputFile}[/]");
        }

        return Success;
    }
}