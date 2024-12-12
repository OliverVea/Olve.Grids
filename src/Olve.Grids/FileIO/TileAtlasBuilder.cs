using Olve.Grids.Brushes;
using Olve.Grids.Generation;
using Olve.Grids.Grids;

namespace Olve.Grids.FileIO;

public class TileAtlasBuilder
{
    public string FilePath { get; }
    public Size ImageSize { get; }
    
    private Size? _tileSize;
    public Size TileSize => _tileSize ?? throw new InvalidOperationException("Tile size must be set.");

    public (int X, int Y) Offset { get; private set; } = new(0, 0);

    private int? _columns;
    public int Columns => _columns ?? (_tileSize == null
        ? throw new InvalidOperationException("Either columns or tile size must be set.")
        : (ImageSize.Width - Offset.X) / TileSize.Width);
    
    private int? _rows;
    public int Rows => _rows ?? (_tileSize == null
        ? throw new InvalidOperationException("Either rows or tile size must be set.")
        : (ImageSize.Height - Offset.Y) / TileSize.Height);

    private TileIndex? _fallbackTileIndex;
    private (int X, int Y)? _fallbackTileIndexXY;
    
    public TileIndex? FallbackTileIndex => _fallbackTileIndex ?? (_fallbackTileIndexXY is {} xy
        ? GridConfiguration.GetTileIndex(xy.X, xy.Y)
        : null);
    
    private GridConfiguration? _gridConfiguration;
    public GridConfiguration GridConfiguration => _gridConfiguration ?? BuildGridConfiguration();
    
    public BrushLookupBuilder BrushLookupBuilder { get; } = new();
    
    private TileAtlasBuilder(string filePath, Size imageSize)
    {
        FilePath = filePath;
        ImageSize = imageSize;
    }
    
    public static TileAtlasBuilder Create(string filePath)
    {
        var imageSize = GetImageSize(filePath);
        return Create(filePath, imageSize);
    }

    private static TileAtlasBuilder Create(string filePath, Size imageSize)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException("File not found.", filePath);
        }
        
        return new TileAtlasBuilder(filePath, imageSize);
    }
    
    public TileAtlasBuilder WithTileSize(Size tileSize)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(tileSize.Width, nameof(tileSize.Width));
        ArgumentOutOfRangeException.ThrowIfNegative(tileSize.Height, nameof(tileSize.Height));
        
        _tileSize = tileSize;
        _gridConfiguration = null;
        return this;
    }
    
    public TileAtlasBuilder WithColumns(int columns)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(columns, nameof(columns));
        
        _columns = columns;
        _gridConfiguration = null;
        return this;
    }
    
    public TileAtlasBuilder WithRows(int rows)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(rows, nameof(rows));
        
        _rows = rows;
        _gridConfiguration = null;
        return this;
    }
    
    public TileAtlasBuilder WithOffset(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x, nameof(x));
        ArgumentOutOfRangeException.ThrowIfNegative(y, nameof(y));
        
        Offset = (x, y);
        _gridConfiguration = null;
        return this;
    }
    
    public TileAtlasBuilder WithFallbackTileIndex(TileIndex fallbackTileIndex)
    {
        _fallbackTileIndex = fallbackTileIndex;
        _fallbackTileIndexXY = null;
        return this;
    }
    
    public TileAtlasBuilder WithFallbackTileIndex(int x, int y)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(x, nameof(x));
        ArgumentOutOfRangeException.ThrowIfNegative(y, nameof(y));
        
        _fallbackTileIndex = null;
        _fallbackTileIndexXY = (x, y);
        return this;
    }
    
    public TileAtlasBuilder WithGridConfiguration(GridConfiguration gridConfiguration)
    {
        _gridConfiguration = gridConfiguration;
        _tileSize = gridConfiguration.TileSize;
        _columns = gridConfiguration.Columns;
        _rows = gridConfiguration.Rows;
        Offset = gridConfiguration.Offset;
        return this;
    }

    private GridConfiguration BuildGridConfiguration()
    {
        if (_tileSize is not {} tileSize) throw new InvalidOperationException("Tile size must be set.");
        
        var tileAtlasXTo = tileSize.Width * Columns + Offset.X;
        var tileAtlasYTo = tileSize.Height * Rows + Offset.Y;
        
        if (tileAtlasXTo > ImageSize.Width || tileAtlasYTo > ImageSize.Height)
        {
            throw new InvalidOperationException("Tile atlas does not fit within image.");
        }
        
        return new GridConfiguration(tileSize, Offset, Rows, Columns);
    }
    
    public TileAtlas Build()
    {
        var gridConfiguration = GridConfiguration;
        var brushLookup = BrushLookupBuilder.Build();
        
        if (FallbackTileIndex is {} fallbackTileIndex)
        {
            if (fallbackTileIndex.Index >= gridConfiguration.TileCount || fallbackTileIndex.Index < 0)
            {
                throw new InvalidOperationException("Fallback tile index is out of bounds.");
            }
            
            return new TileAtlas(FilePath, gridConfiguration, brushLookup)
            {
                FallbackTileIndex = fallbackTileIndex
            };
        }
        
        
        return new TileAtlas(FilePath, gridConfiguration, brushLookup);
        
    }
    
    private static Size GetImageSize(string fileName)
    {
        var br = new BinaryReader(File.OpenRead(fileName));
        
        br.BaseStream.Position = 16;
        var widthBytes = new byte[sizeof(int)];
        for (var i = 0; i < sizeof(int); i++) widthBytes[sizeof(int) - 1 - i ] = br.ReadByte();
        var width = BitConverter.ToInt32(widthBytes, 0);
        
        var heightBytes = new byte[sizeof(int)];
        for (var i = 0; i < sizeof(int); i++) heightBytes[sizeof(int) - 1 - i] = br.ReadByte();
        var height = BitConverter.ToInt32(heightBytes, 0);
        
        return new Size(width, height);
    }
    }