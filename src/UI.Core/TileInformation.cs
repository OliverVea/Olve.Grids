using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using SixLabors.ImageSharp;
using UI.Core.Projects;

namespace UI.Core;

public readonly record struct TileInformation(
    Id<Project> ProjectId,
    TileIndex TileIndex,
    Image Image,
    CornerBrushes CornerBrushes,
    bool Active);