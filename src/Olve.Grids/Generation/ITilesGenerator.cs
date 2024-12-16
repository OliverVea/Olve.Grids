using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using OneOf.Types;

namespace Olve.Grids.Generation;

public record GenerationRequest(TileAtlas TileAtlas, BrushGrid BrushGrid)
{
    public Size OutputSize { get; } = new(BrushGrid.Size.Width - 1, BrushGrid.Size.Height - 1);
}

public record GenerationResult(GenerationRequest Request, TileIndex[,] Tiles, OneOf<Success, Waiting, Error> Status);

public interface IGenerator : IOperation<GenerationRequest, GenerationResult>;
public interface IAsyncGenerator : IAsyncOperation<GenerationRequest, GenerationResult>;