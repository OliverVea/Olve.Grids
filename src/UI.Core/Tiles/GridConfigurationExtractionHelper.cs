using Olve.Grids.Grids;
using UI.Core.Projects;

namespace UI.Core.Tiles;

public static class GridConfigurationExtractionHelper
{
    public static Result<GridConfiguration> GetGridConfiguration(Project project)
    {
        var config = project.TileAtlasBuilder.Configuration;
        var columns = config.Columns;

        if (!columns.HasValue)
        {
            if (!config.ImageSize.HasValue || !config.TileSize.HasValue)
            {
                return new ResultProblem("No columns defined and image size or tile size not defined")
                {
                    Source = nameof(GridConfigurationExtractionHelper),
                };
            }

            columns = config.ImageSize.Value.Width / config.TileSize.Value.Width;
        }

        var rows = config.Rows;

        if (!rows.HasValue)
        {
            if (!config.ImageSize.HasValue || !config.TileSize.HasValue)
            {
                return new ResultProblem("No rows defined and image size or tile size not defined")
                {
                    Source = nameof(GridConfigurationExtractionHelper),
                };
            }

            rows = config.ImageSize.Value.Height / config.TileSize.Value.Height;
        }

        if (!config.TileSize.HasValue)
        {
            return new ResultProblem("No rows defined")
            {
                Source = nameof(GridConfigurationExtractionHelper),
            };
        }

        return new GridConfiguration(config.TileSize.Value, rows.Value, columns.Value);
    }
}