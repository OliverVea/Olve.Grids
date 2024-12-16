using FluentValidation;

namespace Olve.Grids.IO.TileAtlasBuilder;

internal class TileAtlasConfigurationValidator : AbstractValidator<TileAtlasConfiguration>
{
    public TileAtlasConfigurationValidator()
    {
        Size imageSize = new();

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty.")
            .Must(filePath => filePath!.EndsWith(".png"))
            .WithMessage("{PropertyName} must be a PNG file.")
            .Must(File.Exists)
            .WithMessage("{PropertyName} must exist.")
            .Must(filePath => ImageSizeHelper.TryGetImageSize(filePath!, out imageSize))
            .WithMessage("{PropertyName} must be a valid image file.");

        RuleFor(x => x.TileSize)
            .NotNull()
            .WithMessage("{PropertyName} must not be null.")
            .Must(tileSize => tileSize!.Value is { Width: > 0, Height: > 0, })
            .WithMessage("{PropertyName} must be greater than 0.")
            .Must(x => x!.Value.Width <= imageSize.Width && x.Value.Height <= imageSize.Height)
            .WithMessage("{PropertyName} must be less than or equal to the image size.");

        RuleFor(x => x.Columns)
            .GreaterThan(0)
            .When(x => x.Columns is { })
            .WithMessage("{PropertyName} must be greater than 0.");

        RuleFor(x => x.Rows)
            .GreaterThan(0)
            .When(x => x.Rows is { })
            .WithMessage("Rows must be greater than 0.");

        RuleFor(x => x.FallbackTileIndex)
            .Must(x => x?.Index is >= 0)
            .When(x => x.FallbackTileIndex is { })
            .WithMessage("{PropertyName} must be greater than or equal to 0.");

        RuleFor(x => x.AdjacencyLookupBuilder)
            .NotNull()
            .WithMessage("{PropertyName} must be specified.");

        RuleFor(x => x.BrushLookupBuilder)
            .NotNull()
            .WithMessage("{PropertyName} must be specified.");
    }
}