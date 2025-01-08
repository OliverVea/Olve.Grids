using Olve.Grids.Serialization;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Demo.Commands;

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

        var imageLoader = new ImageLoader();
        if (!imageLoader
                .LoadImage(settings.TileAtlasFile)
                .TryPickValue(out var tileAtlasImage, out var imageLoaderError))
        {
            AnsiConsole.MarkupLine($"[bold red]Error:[/] {imageLoaderError}");
            return Error;
        }

        var tileAtlasSize = new Size(tileAtlasImage.Width, tileAtlasImage.Height);

        var tileAtlasLoader = new TileAtlasLoader();
        if (!tileAtlasLoader
                .LoadTileAtlas(tileAtlasSize,
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