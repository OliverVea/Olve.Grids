using MemoryPack;
using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableAdjacencyLookup
{
    public IEnumerable<(TileIndex from, TileIndex to, Direction direction)> Adjacencies { get; set; } = [ ];

    public static SerializableAdjacencyLookup FromAdjacencyLookup(IReadOnlyAdjacencyLookup adjacencyLookup) =>
        new()
        {
            Adjacencies = adjacencyLookup.Adjacencies,
        };

    public FrozenAdjacencyLookup ToFrozenAdjacencyLookup() => new(Adjacencies);
    public AdjacencyLookup ToAdjacencyLookup() => new(Adjacencies);
}