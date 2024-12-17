using Olve.Utilities.Operations;

namespace Olve.Grids.Generation;

public interface IAsyncGenerator : IAsyncOperation<GenerationRequest, GenerationResult>;