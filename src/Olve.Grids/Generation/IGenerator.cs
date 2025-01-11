namespace Olve.Grids.Generation;

public interface IGenerator
{
    GenerationResult Execute(GenerationRequest request);
}