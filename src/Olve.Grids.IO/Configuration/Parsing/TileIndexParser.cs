using Olve.Grids.Grids;
using Olve.Utilities.CollectionExtensions;

namespace Olve.Grids.IO.Configuration.Parsing;

public class TileIndexParser : IParser<string?, IEnumerable<TileIndex>>
{
    private const char Separator = ',';
    private const char SpanSeparator = '-';

    // 13, 23-25
    public OneOf<IEnumerable<TileIndex>, FileParsingError> Parse(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return FileParsingError.New("Tiles are required.");
        }

        var tiles = input
            .Split(Separator)
            .Select(ParseListItem)
            .ToArray();

        if (tiles.AnyT1())
        {
            var errors = tiles.OfT1();
            return FileParsingError.Combine(errors);
        }

        return tiles
            .OfT0()
            .SelectMany(x => x)
            .ToArray();
    }

    private OneOf<IEnumerable<TileIndex>, FileParsingError> ParseListItem(string item)
    {
        if (item.Contains(SpanSeparator))
        {
            return ParseSpan(item);
        }

        return ParseTileIndex(item)
            .Match<OneOf<IEnumerable<TileIndex>, FileParsingError>>(x => new[] { x, }, x => x);
    }

    private OneOf<IEnumerable<TileIndex>, FileParsingError> ParseSpan(string? span)
    {
        if (span is null)
        {
            return FileParsingError.New("Span is required.");
        }

        var tiles = span.Split(SpanSeparator);
        if (tiles.Length != 2)
        {
            return FileParsingError.New("Span must contain two tiles.");
        }

        if (!ParseTileIndex(tiles[0])
                .TryPickT0(out var start, out var parsingError)
            || !ParseTileIndex(tiles[1])
                .TryPickT0(out var end, out parsingError))
        {
            return parsingError;
        }

        if (start.Index > end.Index)
        {
            return FileParsingError.New("Start tile must be less than or equal to end tile.");
        }

        return OneOf<IEnumerable<TileIndex>, FileParsingError>.FromT0(TileIndex.Span(start, end));
    }

    private OneOf<TileIndex, FileParsingError> ParseTileIndex(string? tile)
    {
        if (!int.TryParse(tile, out var tileIndex))
        {
            return FileParsingError.New($"'{tile}' is not a valid tile index.");
        }

        return ParseTileIndex(tileIndex);
    }

    private OneOf<TileIndex, FileParsingError> ParseTileIndex(int? tile)
    {
        if (tile is not { } tileIndex)
        {
            return FileParsingError.New("Tile is required.");
        }

        if (tileIndex < 0)
        {
            return FileParsingError.New("Tile must be non-negative.");
        }

        return new TileIndex(tileIndex);
    }
}