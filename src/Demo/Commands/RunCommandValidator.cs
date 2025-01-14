using FluentValidation;

namespace Demo.Commands;

public class RunCommandValidator : AbstractValidator<RunCommandSettings>
{
    public RunCommandValidator()
    {
        RuleFor(x => x.TileAtlasFile)
            .NotEmpty()
            .WithMessage("The tile atlas file must be specified.")
            .Must(filePath => filePath!.EndsWith(".png"))
            .WithMessage("The tile atlas file must be a PNG file.")
            .Must(File.Exists)
            .WithMessage("The tile atlas file does not exist.");

        RuleFor(x => x.TileSize)
            .NotEmpty()
            .WithMessage("The tile size must be specified.")
            .Must(tileSize => SizeParser.Parse(tileSize)
                .Succeeded)
            .WithMessage(x => SizeParser
                                  .Parse(x.TileSize)
                                  .Problems?.First()
                                  .ToString()
                              ?? "Problem parsing the tile size.");

        RuleFor(x => x.TileAtlasBrushesFile)
            .NotEmpty()
            .WithMessage("The tile atlas brushes file must be specified.")
            .Must(File.Exists)
            .WithMessage("The tile atlas brushes file does not exist.");

        RuleFor(x => x.InputBrushesFile)
            .NotEmpty()
            .WithMessage("The input brushes file must be specified.")
            .Must(File.Exists)
            .WithMessage("The input brushes file does not exist.");

        RuleFor(x => x.OutputFile)
            .NotEmpty()
            .WithMessage("The output file must be specified.")
            .Must(filePath => filePath!.EndsWith(".png"))
            .WithMessage("The output file must be a PNG file.");

        RuleFor(x => new
            {
                x.OutputFile,
                x.Overwrite,
            })
            .Must(filePath => !File.Exists(filePath.OutputFile) || filePath.Overwrite)
            .WithMessage("The output file already exists. Use the '--overwrite' option to overwrite it.");

        RuleFor(x => x.Verbosity)
            .NotEmpty()
            .WithMessage("The verbosity level must be specified.")
            .Must(verbosity => VerbosityLevels.Parse(verbosity)
                .Succeeded)
            .WithMessage(x => VerbosityLevels
                                  .Parse(x.Verbosity)
                                  .Problems?.First()
                                  .ToString()
                              ?? "Problem parsing the verbosity level.");
    }
}