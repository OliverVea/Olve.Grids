using System.Diagnostics.CodeAnalysis;
using FluentValidation.Results;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.Weights;

namespace Olve.Grids.IO.TileAtlasBuilder;

public class TileAtlasBuilder
{
    private readonly TileAtlasConfigurationValidator _validator = new();

    private ValidationResult? _validationResult;

    public TileAtlasConfiguration Configuration { get; } = new();

    public ValidationResult ValidationResult =>
        _validationResult ??= _validator.Validate(Configuration);

    public TileAtlasBuilder WithFilePath(string filePath)
    {
        return Modify(config => config.FilePath = filePath);
    }

    public TileAtlasBuilder WithTileSize(Size tileSize)
    {
        return Modify(config => config.TileSize = tileSize);
    }

    public TileAtlasBuilder WithColumns(int columns)
    {
        return Modify(config => config.Columns = columns);
    }

    public TileAtlasBuilder WithRows(int rows)
    {
        return Modify(config => config.Rows = rows);
    }

    public TileAtlasBuilder WithFallbackTileIndex(TileIndex fallbackTileIndex)
    {
        return Modify(config => config.FallbackTileIndex = fallbackTileIndex);
    }

    public TileAtlasBuilder WithAdjacencyLookupBuilder(
        IAdjacencyLookup adjacencyLookup
    )
    {
        return Modify(config => config.AdjacencyLookup = adjacencyLookup);
    }

    public TileAtlasBuilder ConfigureAdjacencyLookupBuilder(
        Action<IAdjacencyLookup?> configurationAction
    )
    {
        return Modify(config => configurationAction(config.AdjacencyLookup));
    }

    public TileAtlasBuilder WithBrushLookupBuilder(IBrushLookup brushLookup)
    {
        return Modify(config => config.BrushLookup = brushLookup);
    }

    public TileAtlasBuilder ConfigureBrushLookupBuilder(
        Action<IBrushLookup?> configurationAction
    )
    {
        return Modify(config => configurationAction(config.BrushLookup));
    }

    public TileAtlasBuilder WithWeightLookupBuilder(IWeightLookup weightLookup)
    {
        return Modify(config => config.WeightLookup = weightLookup);
    }

    public TileAtlasBuilder ConfigureWeightLookupBuilder(
        Action<IWeightLookup?> configurationAction
    )
    {
        return Modify(config => configurationAction(config.WeightLookup));
    }

    public TileAtlas Build()
    {
        if (!ValidationResult.IsValid)
        {
            throw new InvalidOperationException("Cannot build a tile atlas with invalid configuration.");
        }

        var filePath = Configuration.FilePath ?? throw new InvalidOperationException("File path must be set.");

        var imageSize = ImageSizeHelper.GetImageSize(filePath);
        var tileSize = Configuration.TileSize ?? throw new InvalidOperationException("Tile size must be set.");

        var gridConfiguration = new GridConfiguration(
            tileSize,
            Configuration.Rows ?? imageSize.Height / tileSize.Height,
            Configuration.Columns ?? imageSize.Width / tileSize.Width
        );

        ThrowIfNull(Configuration.BrushLookup, "Brush lookup builder must be set.");
        var frozenBrushLookup = new FrozenBrushLookup(Configuration.BrushLookup.Entries);

        ThrowIfNull(Configuration.AdjacencyLookup, "Adjacency lookup must be set.");
        var frozenAdjacencyLookup = new FrozenAdjacencyLookup(Configuration.AdjacencyLookup.Adjacencies);

        ThrowIfNull(Configuration.WeightLookup, "Weight lookup builder must be set.");
        var frozenWeightLookup = new FrozenWeightLookup(Configuration.WeightLookup.Weights);

        return new TileAtlas(
            filePath,
            gridConfiguration,
            frozenBrushLookup,
            frozenAdjacencyLookup,
            frozenWeightLookup
        )
        {
            FallbackTile = Configuration.FallbackTileIndex ?? new TileIndex(gridConfiguration.TileCount - 1),
        };
    }

    private void ThrowIfNull([NotNull] object? value, string message)
    {
        if (value is null)
        {
            throw new InvalidOperationException(message);
        }
    }

    private TileAtlasBuilder Modify(Action<TileAtlasConfiguration> modifyAction)
    {
        modifyAction(Configuration);

        _validationResult = null;

        return this;
    }
}