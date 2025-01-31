namespace Olve.Grids.Grids;

public static class GridGeometryHelper
{
    /// <summary>
    ///     Returns the origins (top-left corner) of each cell in a grid.
    /// </summary>
    /// <param name="cellSize">The size of each cell.</param>
    /// <param name="rows">The number of rows in the grid.</param>
    /// <param name="cols">The number of columns in the grid.</param>
    /// <param name="scale">The scale factor to apply to the grid.</param>
    /// <returns></returns>
    public static IEnumerable<GridPoint> GetGridCellOrigins(Size cellSize, int rows, int cols, float scale = 1)
    {
        for (var row = 0; row < rows; row++)
        {
            for (var col = 0; col < cols; col++)
            {
                yield return new GridPoint(col * cellSize.Width * scale, row * cellSize.Height * scale);
            }
        }
    }

    public static IEnumerable<GridLine> GetGridLines(Size cellSize, int rows, int cols, float scale = 1)
    {
        for (var row = 0; row <= rows; row++)
        {
            yield return new GridLine(
                new GridPoint(0, row * cellSize.Height * scale),
                new GridPoint(cols * cellSize.Width * scale, row * cellSize.Height * scale)
            );
        }

        for (var col = 0; col <= cols; col++)
        {
            yield return new GridLine(
                new GridPoint(col * cellSize.Width * scale, 0),
                new GridPoint(col * cellSize.Width * scale, rows * cellSize.Height * scale)
            );
        }
    }
}