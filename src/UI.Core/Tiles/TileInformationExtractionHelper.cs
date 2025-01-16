using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Weights;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using UI.Core.Projects;

namespace UI.Core.Tiles;

public static class TileInformationExtractionHelper
{
    public static Result<TileInformation> ExtractTileInformation(Project project, TileIndex tileIndex)
    {
        var (row, col) = project.GridConfiguration.GetRowAndColumn(tileIndex);

        var image = project.TileSheetImage.Image.Clone(x => x.Crop(new Rectangle(
            col * project.GridConfiguration.TileSize.Width,
            row * project.GridConfiguration.TileSize.Height,
            project.GridConfiguration.TileSize.Width,
            project.GridConfiguration.TileSize.Height)));

        var weight = project.WeightLookup.GetWeight(tileIndex);

        var cornerBrushes = project
            .BrushLookup
            .GetBrushes(tileIndex)
            .GetT0OrDefault(new CornerBrushes());
        var neighbors = project.AdjacencyLookup.GetNeighbors(tileIndex);
        var active = project.ActiveTiles.Contains(tileIndex);

        return new TileInformation(tileIndex, image, weight, cornerBrushes, neighbors, active);
    }
}