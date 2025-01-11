using DeBroglie;
using DeBroglie.Models;
using DeBroglie.Topo;
using Olve.Grids.Generation;

using Direction = Olve.Grids.Primitives.Direction;

namespace Olve.Grids.DeBroglie;

public class DeBroglieGenerator : IGenerator
{
    public GenerationResult Execute(GenerationRequest request)
    {
        GenerationResult? result = null;

        for (var attempt = 1; attempt <= request.Attempts; attempt++)
        {
            result = ExecuteInternal(request, attempt == request.Attempts);
            if (result.Result.Succeded)
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

        var tileIndices = request
            .TileAtlas.Grid.GetTileIndices()
            .ToArray();

        foreach (var tileIndex in tileIndices)
        {
            var frequency = request.TileAtlas.WeightLookup.GetWeight(tileIndex);
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
        };

        var propagator = new TilePropagator(model, topology, propagatorOptions);

        var resolution = propagator.Run();
        var status = GetStatus(resolution);

        var result = propagator
            .ToArray()
            .Map(x => ((Tile?)x).ToTileIndex(request.TileAtlas.FallbackTile))
            .ToArray2d();

        for (var i = 0; i < request.OutputSize.Width && status.Succeded; i++)
        {
            for (var j = 0; j < request.OutputSize.Height; j++)
            {
                if (result[i, j] == request.TileAtlas.FallbackTile)
                {
                    status = Result.Failure(new ResultProblem("Output contained fallback tile"));
                }
            }
        }

        return new GenerationResult(request, result, status);
    }

    private static Result GetStatus(Resolution resolution)
    {
        return resolution switch
        {
            Resolution.Decided => Result.Success(),
            Resolution.Undecided => Result.Failure(new ResultProblem("Generation is undecided.")),
            Resolution.Contradiction => Result.Failure(new ResultProblem("Generation has contradiction")),
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

        foreach (var direction in Direction.All.GetDeBroglieDirections())
        {
            model.AddAdjacency([ fallbackTile, ], allTiles, direction);
        }

        model.SetFrequency(fallbackTile, 1e-20);
    }
}