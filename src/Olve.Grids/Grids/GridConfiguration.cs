namespace Olve.Grids.Grids;

public readonly record struct GridConfiguration(Size TileSize, int Rows, int Columns)
{
    public int TileCount => Rows * Columns;

    public IEnumerable<TileIndex> GetTileIndices()
    {
        var tileCount = TileCount;
        for (var i = 0; i < tileCount; i++)
        {
            yield return new TileIndex(i);
        }
    }

    public TileIndex GetTileIndex(int row, int column)
    {
        if (Rows == 0 || Columns == 0)
        {
            throw new InvalidOperationException(
                "Cannot get a tile index from a grid with no rows or columns."
            );
        }

        ArgumentOutOfRangeException.ThrowIfNegative(row, nameof(row));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(row, Rows, nameof(row));
        ArgumentOutOfRangeException.ThrowIfNegative(column, nameof(column));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(column, Columns, nameof(column));

        return new TileIndex(row * Columns + column);
    }

    public (int Row, int Column) GetRowAndColumn(TileIndex tileIndex)
    {
        if (Rows == 0 || Columns == 0)
        {
            throw new InvalidOperationException(
                "Cannot get a row and column from a grid with no rows or columns."
            );
        }

        ArgumentOutOfRangeException.ThrowIfNegative(tileIndex.Index, nameof(tileIndex));
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(
            tileIndex.Index,
            TileCount,
            nameof(tileIndex)
        );

        var row = tileIndex.Index / Columns;
        var column = tileIndex.Index % Columns;

        return (row, column);
    }
}