using DeBroglie.Models;
using Olve.Grids.Generation;

namespace Olve.Grids.DeBroglie;

public class TileAtlasAdjacencyBuilder
{
    public IReadOnlyCollection<AdjacentModel.Adjacency> BuildAdjacencies(TileAtlas tileAtlas)
    {
        var adjacencies = new List<AdjacentModel.Adjacency>();

        foreach (var tileIndex in tileAtlas.Grid.GetTileIndices())
        {
            var neighbors = tileAtlas
                .AdjacencyLookup
                .GetNeighbors(tileIndex)
                .Select(x => (tileIndex: x, direction: tileAtlas.AdjacencyLookup.Get(tileIndex, x)))
                .ToArray();

            if (neighbors.Length == 0)
            {
                continue;
            }

            var src = tileIndex.ToTile();

            foreach (var (neighbor, adjacencyDirection) in neighbors)
            {
                foreach (var direction in adjacencyDirection.GetDeBroglieDirections())
                {
                    var adjacency = new AdjacentModel.Adjacency
                    {
                        Src = [src],
                        Dest = [neighbor.ToTile()],
                        Direction = direction,
                    };

                    adjacencies.Add(adjacency);
                }
            }
        }

        return adjacencies;
    }
}