using Olve.Grids.Brushes;

namespace Olve.Grids.Generation;

public class BrushGrid
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

    public IEnumerable<(Position position, OneOf<BrushId, Any>)> GetPositions()
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

    private void VerifyPosition(Position position)
    {
        var (x, y) = position;

        ArgumentOutOfRangeException.ThrowIfNegative(x);
        ArgumentOutOfRangeException.ThrowIfNegative(y);

        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(x, Size.Width);
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(y, Size.Height);
    }

    private int GetIndex(Position position) => position.Y * Size.Width + position.X;

    private static OneOf<BrushId, Any> GetBrushOrAny(BrushId? brushId) => brushId ?? OneOf<BrushId, Any>.FromT1(new Any());

    private static BrushId? GetBrushId(OneOf<BrushId, Any> brush) =>
        brush.TryPickT0(out var brushId, out _) ? brushId : null;

}