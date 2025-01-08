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

    public static FrozenAdjacencyLookup ToFrozenAdjacencyLookup(SerializableAdjacencyLookup serializableAdjacencyLookup) =>
        new(serializableAdjacencyLookup.Adjacencies);

    public static AdjacencyLookup ToAdjacencyLookup(SerializableAdjacencyLookup serializableAdjacencyLookup) =>
        new(serializableAdjacencyLookup.Adjacencies);
}