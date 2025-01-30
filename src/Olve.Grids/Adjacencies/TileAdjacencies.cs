using System.Collections;

namespace Olve.Grids.Adjacencies;

public class TileAdjacencies(IEnumerable<TileAdjacency> items) : IEnumerable<TileAdjacency>
{
    public IEnumerable<TileAdjacency> Items { get; } = items;

    public IEnumerator<TileAdjacency> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static TileAdjacencies FromEnumerable(IEnumerable<TileAdjacency> items) => new(items);
}