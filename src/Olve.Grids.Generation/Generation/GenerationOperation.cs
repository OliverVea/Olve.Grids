using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Topo;
using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Utilities.Operations;
using Olve.Utilities.Types;
using OneOf.Types;

namespace Olve.Grids.Generation.Generation;

public record GenerationRequest(TileAtlas.TileAtlas TileAtlas, BrushGrid BrushGrid)
{
    public Size OutputSize { get; } = new(BrushGrid.Size.Width - 1, BrushGrid.Size.Height - 1);
}

public record GenerationResult(GenerationRequest Request, TileIndex[,] Tiles, OneOf<Success, Waiting, Error> Status);

public class GenerationOperation : IOperation<GenerationRequest, GenerationResult>
{
    public GenerationResult Execute(GenerationRequest request)
    {
        var constraintBuilder = new TileAtlasConstraintBuilder();
        var constraints = constraintBuilder.BuildConstraints(request.TileAtlas, request.BrushGrid);

        var adjacencyBuilder = new TileAtlasAdjacencyBuilder();
        var adjacencies = adjacencyBuilder.BuildAdjacencies(request.TileAtlas);
        
        var model = new AdjacentModel();
        model.SetDirections(DirectionSet.Cartesian2d);
        
        foreach (var adjacency in adjacencies)
        {
            model.AddAdjacency(adjacency.Src, adjacency.Dest, adjacency.Direction);
        }

        foreach (var (tileIndex, frequency) in request.TileAtlas.WeightLookup)
        {
            model.SetFrequency(tileIndex.ToTile(), frequency);
        }

        //AddFallbackTile(model, request.TileAtlas);
        
        var topology = new GridTopology(request.OutputSize.Width, request.OutputSize.Height, false);

        var propagatorOptions = new TilePropagatorOptions
        {
            Constraints = constraints.ToArray(),
            BacktrackType = BacktrackType.Backjump,
            IndexPickerType = IndexPickerType.Default,
            TilePickerType = TilePickerType.Default,
        };

        var propagator = new TilePropagator(model, topology, propagatorOptions);

        var resolution = propagator.Run();
        var status = GetStatus(resolution);

        var result = propagator.ToValueArray<int>().Map(x => new TileIndex(x)).ToArray2d();

        return new GenerationResult(request, result, status);
    }
    
    private static OneOf<Success, Waiting, Error> GetStatus(Resolution resolution)
    {
        return resolution switch
        {
            Resolution.Decided => new Success(),
            Resolution.Undecided => new Waiting(),
            Resolution.Contradiction => new Error(),
            _ => throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null)
        };
    }
    
    private static void AddFallbackTile(AdjacentModel model, TileAtlas.TileAtlas tileAtlas)
    {
        var fallbackTile = tileAtlas.FallbackTileIndex.ToTile();
    
        var allTiles = tileAtlas.Grid.GetTileIndices().Select(x => x.ToTile()).ToArray();
    
        foreach (var direction in AdjacencyDirection.All.GetDeBroglieDirections())
        {
            model.AddAdjacency([fallbackTile], allTiles, direction);
        }
    
        model.SetFrequency(fallbackTile, 1e-20);
    }
}