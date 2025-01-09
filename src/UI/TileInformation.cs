using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using SixLabors.ImageSharp;

namespace UI;

public readonly record struct TileInformation(
    TileIndex TileIndex,
    Image Image,
    float Weight,
    CornerBrushes CornerBrushes,
    IEnumerable<(TileIndex tileIndex, Direction direction)> Adjacencies,
    bool Active);