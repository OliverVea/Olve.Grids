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
        var gridConfigurationResult = GridConfigurationExtractionHelper.GetGridConfiguration(project);
        if (gridConfigurationResult.TryPickProblems(out var problems, out var gridConfiguration))
        {
            return problems;
        }

        var (row, col) = gridConfiguration.GetRowAndColumn(tileIndex);

        var image = project.TileSheetImage.Image.Clone(x => x.Crop(new Rectangle(
            col * gridConfiguration.TileSize.Width,
            row * gridConfiguration.TileSize.Height,
            gridConfiguration.TileSize.Width,
            gridConfiguration.TileSize.Height)));

        var weightLookup = project.TileAtlasBuilder.Configuration.WeightLookup ?? new WeightLookup();
        var brushLookup = project.TileAtlasBuilder.Configuration.BrushLookup ?? new BrushLookup();
        var adjacencyLookup = project.TileAtlasBuilder.Configuration.AdjacencyLookup ?? new AdjacencyLookup();

        var weight = weightLookup.GetWeight(tileIndex);


        var cornerBrushes = brushLookup
            .GetBrushes(tileIndex)
            .GetT0OrDefault(new CornerBrushes());
        var neighbors = adjacencyLookup.GetNeighbors(tileIndex);
        var active = project.ActiveTiles.Contains(tileIndex);

        return new TileInformation(tileIndex, image, weight, cornerBrushes, neighbors, active);
    }
}