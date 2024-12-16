using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Topo;
using Olve.Grids.Adjacencies;
using Olve.Grids.Generation;
using OneOf.Types;

namespace Olve.Grids.DeBroglie;

public class DeBroglieGenerator : IGenerator
{

    public GenerationResult Execute(GenerationRequest request)
    {
        GenerationResult? result = null;

        for (var attempt = 1; attempt <= request.Attempts; attempt++)
        {
            result = ExecuteInternal(request, attempt == request.Attempts);
            if (result.Status.IsT0)
            {
                return result;
            }

            Console.WriteLine($"Generation failed. Retrying... ({request.Attempts - attempt} attempts left)");
        }

        return result ?? throw new InvalidOperationException("Generation failed.");
    }

    private GenerationResult ExecuteInternal(GenerationRequest request, bool fallback = false)
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

        if (fallback)
        {
            AddFallbackTile(model, request.TileAtlas);
        }

        var topology = new GridTopology(request.OutputSize.Width, request.OutputSize.Height, false);

        var propagatorOptions = new TilePropagatorOptions
        {
            Constraints = constraints.ToArray(),
            BacktrackType = BacktrackType.Backtrack,
            MaxBacktrackDepth = 1000,
            IndexPickerType = IndexPickerType.Default,
            TilePickerType = TilePickerType.Default,
        };

        var propagator = new TilePropagator(model, topology, propagatorOptions);

        var resolution = propagator.Run();
        var status = GetStatus(resolution);

        var result = propagator
            .ToArray()
            .Map(x => ((Tile?)x).ToTileIndex(request.TileAtlas.FallbackTile))
            .ToArray2d();

        for (var i = 0; i < request.OutputSize.Width; i++)
        {
            for (var j = 0; j < request.OutputSize.Height; j++)
            {
                if (result[i, j] == request.TileAtlas.FallbackTile)
                {
                    status = new Error();
                }
            }
        }

        return new GenerationResult(request, result, status);
    }

    private static OneOf<Success, Waiting, Error> GetStatus(Resolution resolution)
    {
        return resolution switch
        {
            Resolution.Decided => new Success(),
            Resolution.Undecided => new Waiting(),
            Resolution.Contradiction => new Error(),
            _ => throw new ArgumentOutOfRangeException(nameof(resolution), resolution, null),
        };
    }

    private static void AddFallbackTile(AdjacentModel model, TileAtlas tileAtlas)
    {
        var fallbackTile = tileAtlas.FallbackTile.ToTile();

        var allTiles = tileAtlas
            .Grid.GetTileIndices()
            .Select(x => x.ToTile())
            .ToArray();

        foreach (var direction in AdjacencyDirection.All.GetDeBroglieDirections())
        {
            model.AddAdjacency([ fallbackTile, ], allTiles, direction);
        }

        model.SetFrequency(fallbackTile, 1e-20);
    }
}