using System.Collections;

namespace Olve.Grids.Brushes;

public class TileBrushes(IEnumerable<TileBrush> items)
    : IEnumerable<TileBrush>
{
    public IEnumerable<TileBrush> Items { get; } = items;

    public IEnumerator<TileBrush> GetEnumerator() => Items.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public static TileBrushes FromEnumerable(IEnumerable<TileBrush> items) => new(items);
}