using Olve.Grids.Grids;

namespace Olve.Grids.IO.Configuration.Parsing;

internal class TileIndexParser : IParser<int?, TileIndex>
{
    public OneOf<TileIndex, FileParsingError> Parse(int? tile)
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