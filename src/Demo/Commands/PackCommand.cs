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
        var verbosityLevelResult = VerbosityLevels.Parse(settings.Verbosity);
        if (verbosityLevelResult.TryPickProblems(out var problems, out var verbosityLevel))
        {
            // Todo: Maybe prepend a problem that helps to show where the problem occured.
            //       e.g.: problems.Prepend(new ProblemResult("Could not parse verbosity level '{0}'", settings.Verbosity);
            problems.LogToAnsiConsole();
            return Error;
        }

        var sizeResult = SizeParser.Parse(settings.TileSize);
        if (sizeResult.TryPickProblems(out problems, out var tileSize))
        {
            problems.LogToAnsiConsole();
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

        var tileAtlasResult = new TileAtlasLoader().LoadTileAtlas(
            tileAtlasSize,
            tileSize,
            settings.TileAtlasBrushesFile,
            settings.TileAtlasConfigFile);
        if (tileAtlasResult.TryPickProblems(out problems, out var tileAtlas))
        {
            problems.LogToAnsiConsole();
            return Error;
        }

        var serializer = new TileAtlasSerializer();
        var saveTileAtlasResult =
            new TileAtlasFileLoader(serializer).Save(tileAtlas, settings.OutputFile, settings.Overwrite);
        if (saveTileAtlasResult.TryPickProblems(out problems))
        {
            problems.LogToAnsiConsole();
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