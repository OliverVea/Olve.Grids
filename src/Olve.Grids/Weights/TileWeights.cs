using System.Collections;

namespace Olve.Grids.Weights;

public class TileWeights(IEnumerable<TileWeight> items)
    : IEnumerable<TileWeight>
{
    public IEnumerable<TileWeight> Items { get; } = items;

    public IEnumerator<TileWeight> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static TileWeights FromEnumerable(IEnumerable<TileWeight> items) => new(items);
}