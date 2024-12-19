using System.ComponentModel;
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