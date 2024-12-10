using Olve.Grids.Generation.Generation;
using Olve.Grids.Grids;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Olve.Grids.Generation;

public class VisualizationExporter
{
    public void ExportAsPng(GenerationResult generationResult, string path)
    {
        var tileAtlas = generationResult.Request.TileAtlas;
        var (outputTilesX, outputTilesY) = generationResult.Request.OutputSize;
        
        var outputWidth = outputTilesX * tileAtlas.Grid.TileSize.Width;
        var outputHeight = outputTilesY * tileAtlas.Grid.TileSize.Height;
        
        var tileAtlasImage = Image.Load(tileAtlas.FilePath);
        var outputImage = new Image<Rgba32>(outputWidth, outputHeight);
        
        for (var y = 0; y < outputTilesY; y++)
        {
            for (var x = 0; x < outputTilesX; x++)
            {
                var tileIndex = generationResult.Tiles[x, y];
                
                var toLocation = GetToLocation(x, y, tileAtlas.Grid.TileSize);
                var fromBox = GetSourceBox(tileAtlas, tileIndex);
                
                outputImage.Mutate(ctx => ctx.DrawImage(tileAtlasImage, toLocation, fromBox, 1));
            }
        }
        
        outputImage.SaveAsPng(path);
    }

    private Rectangle GetSourceBox(TileAtlas.TileAtlas tileAtlas, TileIndex tileIndex) => GetSourceBox(tileAtlas.Grid, tileIndex);

    private Rectangle GetSourceBox(GridConfiguration grid, TileIndex tileIndex)
    {
        var (tileY, tileX) = grid.GetRowAndColumn(tileIndex);
        
        var (tileWidth, tileHeight) = grid.TileSize;
        var (offsetX, offsetY) = grid.Offset;

        var x = offsetX + tileX * tileWidth;
        var y = offsetY + tileY * tileHeight;
        
        return new Rectangle(x, y, tileWidth, tileHeight);
    }

    private Point GetToLocation(int x, int y, Size gridTileSize)
    {
        var (tileWidth, tileHeight) = gridTileSize;
        
        return new Point(x * tileWidth, y * tileHeight);
    }
}