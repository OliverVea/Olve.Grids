using Olve.Grids.Grids;


namespace Olve.Grids.Generation;

public record GenerationResult(GenerationRequest Request, TileIndex[,] Tiles, Result Result);