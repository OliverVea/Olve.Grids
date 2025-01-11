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

    public IEnumerable<(Position position, BrushIdOrAny)> GetPositions()
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

    public BrushIdOrAny GetBrush(Position position)
    {
        VerifyPosition(position);
        var index = GetIndex(position);
        var brushId = _brushes[index];

        return GetBrushOrAny(brushId);
    }

    public void SetBrush(Position position, BrushIdOrAny brushIdOrAny)
    {
        VerifyPosition(position);
        var index = GetIndex(position);

        _brushes[index] = GetBrushId(brushIdOrAny);
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

    private static BrushIdOrAny GetBrushOrAny(BrushId? brushId) => brushId ?? BrushIdOrAny.Any;

    private static BrushId? GetBrushId(BrushIdOrAny brushIdOrAny) =>
        brushIdOrAny.TryPickT0(out var brushId, out _) ? brushId : null;

}