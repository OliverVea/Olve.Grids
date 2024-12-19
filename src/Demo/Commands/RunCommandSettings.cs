using System.ComponentModel;
using Spectre.Console.Cli;

namespace Demo.Commands;

public class RunCommandSettings : CommandSettings
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

    [Description]
    [CommandOption("-c|--tile-atlas-config")]
    public string? TileAtlasConfigFile { get; set; }

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