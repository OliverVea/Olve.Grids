using FluentValidation.Results;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;
using Olve.Grids.IO.Readers;
using Olve.Grids.Weights;
using SixLabors.ImageSharp;

namespace Olve.Grids.IO.TileAtlasBuilder;

public class TileAtlasBuilder
{
    private readonly TileAtlasConfigurationValidator _validator = new();

    public TileAtlasConfiguration Configuration { get; } = new();

    private ValidationResult? _validationResult;
    public ValidationResult ValidationResult =>
        _validationResult ??= _validator.Validate(Configuration);

    public TileAtlasBuilder WithFilePath(string filePath) =>
        Modify(config => config.FilePath = filePath);

    public TileAtlasBuilder WithTileSize(Size tileSize) =>
        Modify(config => config.TileSize = tileSize);

    public TileAtlasBuilder WithColumns(int columns) => Modify(config => config.Columns = columns);

    public TileAtlasBuilder WithRows(int rows) => Modify(config => config.Rows = rows);

    public TileAtlasBuilder WithFallbackTileIndex(TileIndex fallbackTileIndex) =>
        Modify(config => config.FallbackTileIndex = fallbackTileIndex);

    public TileAtlasBuilder WithAdjacencyLookupBuilder(
        IAdjacencyLookupBuilder adjacencyLookupBuilder
    ) => Modify(config => config.AdjacencyLookupBuilder = adjacencyLookupBuilder);

    public TileAtlasBuilder ConfigureAdjacencyLookupBuilder(
        Action<IAdjacencyLookupBuilder?> configurationAction
    ) => Modify(config => configurationAction(config.AdjacencyLookupBuilder));

    public TileAtlasBuilder WithAdjacencyLookupFromFile(string filePath)
    {
        var fileReader = new TileAtlasAdjacenciesFileReader(filePath);

        if (Configuration.BrushLookupBuilder is not { } brushLookupBuilder)
        {
            throw new InvalidOperationException("Brush lookup builder must be configured.");
        }

        var brushLookup = brushLookupBuilder.Build();

        return WithAdjacencyLookupBuilder(fileReader.Read(brushLookup));
    }

    public TileAtlasBuilder WithBrushLookupBuilder(IBrushLookupBuilder brushLookupBuilder) =>
        Modify(config => config.BrushLookupBuilder = brushLookupBuilder);

    public TileAtlasBuilder ConfigureBrushLookupBuilder(
        Action<IBrushLookupBuilder?> configurationAction
    ) => Modify(config => configurationAction(config.BrushLookupBuilder));

    public TileAtlasBuilder WithBrushLookupFromFile(string filePath)
    {
        var fileReader = new TileAtlasBrushesFileReader(filePath);

        if (!fileReader.Load().TryPickT0(out var brushLookupBuilder, out var errors))
        {
            throw errors.ToException();
        }

        return WithBrushLookupBuilder(brushLookupBuilder);
    }

    public TileAtlasBuilder WithWeightLookupBuilder(IWeightLookupBuilder weightLookupBuilder) =>
        Modify(config => config.WeightLookupBuilder = weightLookupBuilder);

    public TileAtlasBuilder WithNewWeightLookupBuilder(
        Action<IWeightLookupBuilder> configurationAction,
        IEnumerable<KeyValuePair<TileIndex, float>>? weights = null,
        float defaultWeight = 1f
    )
    {
        var weightLookupBuilder = new WeightLookup(weights, defaultWeight);

        configurationAction(weightLookupBuilder);

        return WithWeightLookupBuilder(weightLookupBuilder);
    }

    public OneOf<TileAtlas, IList<ValidationFailure>> Build()
    {
        if (!ValidationResult.IsValid)
        {
            return ValidationResult.Errors;
        }

        var filePath =
            Configuration.FilePath ?? throw new InvalidOperationException("File path must be set.");

        var imageSize = ImageSizeHelper.GetImageSize(filePath);
        var tileSize =
            Configuration.TileSize ?? throw new InvalidOperationException("Tile size must be set.");

        var gridConfiguration = new GridConfiguration(
            tileSize,
            Configuration.Rows ?? imageSize.Height / tileSize.Height,
            Configuration.Columns ?? imageSize.Width / tileSize.Width
        );

        var brushLookup =
            Configuration.BrushLookupBuilder?.Build()
            ?? throw new InvalidOperationException("Brush lookup builder must be set.");

        var adjacencyLookup =
            Configuration.AdjacencyLookupBuilder?.Build()
            ?? throw new InvalidOperationException("Adjacency lookup builder must be set.");

        var weightLookup =
            Configuration.WeightLookupBuilder?.Build()
            ?? new WeightLookup(gridConfiguration.GetTileIndices());

        return new TileAtlas(
            filePath,
            gridConfiguration,
            brushLookup,
            adjacencyLookup,
            weightLookup
        )
        {
            FallbackTile =
                Configuration.FallbackTileIndex ?? new TileIndex(gridConfiguration.TileCount - 1),
        };
    }

    private TileAtlasBuilder Modify(Action<TileAtlasConfiguration> modifyAction)
    {
        modifyAction(Configuration);
        _validationResult = null;
        return this;
    }
}
