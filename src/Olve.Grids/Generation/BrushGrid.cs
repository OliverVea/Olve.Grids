using System.Collections;
using Olve.Grids.Brushes;

namespace Olve.Grids.Generation;

public class BrushGrid : IEnumerable<(Position position, OneOf<BrushId, Any>)>
{

    private readonly BrushId?[] _brushes;

    public BrushGrid(Size size, BrushId? initialBrush = null)
    {
        Size = size;
        Count = size.Width * size.Height;

        _brushes = new BrushId?[Count];

        if (initialBrush != null)
        {
            Fill(initialBrush.Value);
        }
    }

    public Size Size { get; }
    public int Count { get; }


    public IEnumerator<(Position position, OneOf<BrushId, Any>)> GetEnumerator()
    {
        for (var y = 0; y < Size.Height; y++)
        {
            for (var x = 0; x < Size.Width; x++)
            {
                var position = new Position(x, y);
                var brush = GetBrush(position);

                yield return (position, brush);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public OneOf<BrushId, Any> GetBrush(Position position)
    {
        VerifyPosition(position);
        var index = GetIndex(position);
        var brushId = _brushes[index];

        return GetBrushOrAny(brushId);
    }

    public void SetBrush(Position position, OneOf<BrushId, Any> brush)
    {
        VerifyPosition(position);
        var index = GetIndex(position);

        _brushes[index] = GetBrushId(brush);
    }

    private void Fill(BrushId brushId)
    {
        for (var i = 0; i < _brushes.Length; i++)
        {
            _brushes[i] = brushId;
        }
    }

    /// <summary>
    ///     Verifies that the position is within the bounds of the grid.
    /// </summary>
    /// <param name="position">The position to verify.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown if the position is outside the bounds of the grid.</exception>
    private void VerifyPosition(Position position)
    {
        var (x, y) = position;

        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(x, Size.Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(y, Size.Height);
    }

    private int GetIndex(Position position)
    {
        return position.Y * Size.Width + position.X;
    }

    private static OneOf<BrushId, Any> GetBrushOrAny(BrushId? brushId)
    {
        return brushId ?? OneOf<BrushId, Any>.FromT1(new Any());
    }

    private static BrushId? GetBrushId(OneOf<BrushId, Any> brush)
    {
        return brush.TryPickT0(out var brushId, out _) ? brushId : null;
    }
}