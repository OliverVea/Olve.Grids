namespace Olve.Grids.Generation;

public record GenerationRequest(TileAtlas TileAtlas, BrushGrid BrushGrid, int Attempts = 10)
{
    public Size OutputSize { get; } = new(BrushGrid.Size.Width - 1, BrushGrid.Size.Height - 1);
}