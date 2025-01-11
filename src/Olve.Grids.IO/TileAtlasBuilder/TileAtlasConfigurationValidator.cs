using FluentValidation;

namespace Olve.Grids.IO.TileAtlasBuilder;

internal class TileAtlasConfigurationValidator : AbstractValidator<TileAtlasConfiguration>
{
    public TileAtlasConfigurationValidator()
    {
        RuleFor(x => x.TileSize)
            .NotNull()
            .WithMessage("{PropertyName} must not be null.")
            .Must(tileSize => tileSize!.Value is { Width: > 0, Height: > 0, })
            .WithMessage("{PropertyName} must be greater than 0.");
        
        RuleFor(x => x.ImageSize)
            .NotNull()
            .WithMessage("{PropertyName} must not be null.");
        
        
        RuleFor(x => new {x.TileSize, x.ImageSize})
            .Must(x => x.TileSize!.Value.Width <= x.ImageSize!.Value.Width && x.TileSize!.Value.Height <= x.ImageSize!.Value.Height)
            .WithMessage("{PropertyName} must be less than or equal to the image size.");

        RuleFor(x => new
            {
                x.Columns,
                x.ImageSize,
            })
            .Must(x => x.Columns is { } || x.ImageSize is { })
            .WithMessage("Either Columns or ImageSize must be specified.");

        RuleFor(x => new
            {
                x.Rows,
                x.ImageSize,
            })
            .Must(x => x.Rows is { } || x.ImageSize is { })
            .WithMessage("Either Rows or ImageSize must be specified.");

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

        RuleFor(x => x.AdjacencyLookup)
            .NotNull()
            .WithMessage("{PropertyName} must be specified.");

        RuleFor(x => x.BrushLookup)
            .NotNull()
            .WithMessage("{PropertyName} must be specified.");
    }
}