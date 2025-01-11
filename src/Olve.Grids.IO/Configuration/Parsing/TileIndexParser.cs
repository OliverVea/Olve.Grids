using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration.Parsing;

public class TileIndexParser : IParser<string?, IEnumerable<TileIndex>>
{
    private const char Separator = ',';
    private const char SpanSeparator = '-';

    // 13, 23-25
    public Result<IEnumerable<TileIndex>> Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            var problem = new ResultProblem("Tiles are required");
            return Result<IEnumerable<TileIndex>>.Failure(problem);
        }

        var tileResults = input.Split(Separator).Select(ParseListItem);

        if (tileResults.TryPickProblems(out var problems, out var tileLists))
        {
            return problems;
        }

        var tiles = tileLists.SelectMany(x => x);

        return Result<IEnumerable<TileIndex>>.Success(tiles);
    }

    private Result<IEnumerable<TileIndex>> ParseListItem(string item)
    {
        if (item.Contains(SpanSeparator))
        {
            return ParseSpan(item);
        }

        if (ParseTileIndex(item)
            .TryPickProblems(out var problems, out var tileIndex))
        {
            return problems;
        }

        return Result<IEnumerable<TileIndex>>.Success([ tileIndex, ]);
    }

    private Result<IEnumerable<TileIndex>> ParseSpan(string? span)
    {
        if (span is null)
        {
            var problem = new ResultProblem("Span is required");
            return Result<IEnumerable<TileIndex>>.Failure(problem);
        }

        var tiles = span.Split(SpanSeparator);
        if (tiles.Length != 2)
        {
            var problem = new ResultProblem("Span '{0}' must contain two tiles, got '{1}'", span, tiles.Length);
            return Result<IEnumerable<TileIndex>>.Failure(problem);
        }

        if (ParseTileIndex(tiles[0]).TryPickProblems(out var problems, out var start)
            || ParseTileIndex(tiles[1]).TryPickProblems(out problems, out var end))
        {
            return Result<IEnumerable<TileIndex>>.Failure(problems);
        }

        if (start.Index > end.Index)
        {
            var problem = new ResultProblem("Start index '{0}' cannot be greater than the end index '{1}'", start, end);
            return Result<IEnumerable<TileIndex>>.Failure(problem);
        }

        return Result<IEnumerable<TileIndex>>.Success(TileIndex.Span(start, end));
    }

    private Result<TileIndex> ParseTileIndex(string? tile)
    {
        if (!int.TryParse(tile, out var tileIndex))
        {
            var problem = new ResultProblem("Value '{0}' is not a valid tile index", tile!);
            return Result<TileIndex>.Failure(problem);
        }

        return ParseTileIndex(tileIndex);
    }

    private Result<TileIndex> ParseTileIndex(int? tile)
    {
        if (tile is not { } tileIndex)
        {
            var problem = new ResultProblem("A tile index cannot be null");
            return Result<TileIndex>.Failure(problem);
        }

        if (tileIndex < 0)
        {
            var problem = new ResultProblem("Tile index '{0}' cannot be negative", tileIndex);
            return Result<TileIndex>.Failure(problem);
        }

        return new TileIndex(tileIndex);
    }
}