using Olve.Grids.Grids;
using OneOf.Types;

namespace Olve.Grids.Generation;

public record GenerationResult(GenerationRequest Request, TileIndex[,] Tiles, OneOf<Success, Waiting, Error> Status);