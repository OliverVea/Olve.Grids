using MemoryPack;
using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Serialization.Models;

[MemoryPackable]
public partial class SerializableAdjacencyLookup
{
    public SerializableTileAdjacency[] Adjacencies { get; set; } = [ ];

    public static SerializableAdjacencyLookup FromAdjacencyLookup(IReadOnlyAdjacencyLookup adjacencyLookup) =>
        new()
        {
            Adjacencies = adjacencyLookup
                .TileAdjacencies.Select(SerializableTileAdjacency.FromTileAdjacency)
                .ToArray(),
        };

    private IEnumerable<TileAdjacency> Items => Adjacencies
        .Select(adjacency => adjacency.ToTileAdjacency());

    public FrozenAdjacencyLookup ToFrozenAdjacencyLookup() => new(Items);
    public AdjacencyLookup ToAdjacencyLookup() => new(Items);
}

[MemoryPackable]
public partial class SerializableTileAdjacency
{
    public required int From { get; set; }
    public required int To { get; set; }
    public required Direction Direction { get; set; }

    public static SerializableTileAdjacency FromTileAdjacency(
        TileAdjacency tileAdjacency) =>
        new()
        {
            From = tileAdjacency.From.Index,
            To = tileAdjacency.To.Index,
            Direction = tileAdjacency.Direction,
        };

    public TileAdjacency ToTileAdjacency() =>
        (new TileIndex(From), new TileIndex(To), Direction);
}