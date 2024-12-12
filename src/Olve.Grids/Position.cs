namespace Olve.Grids;

public readonly record struct Position(int X, int Y)
{
    public static DeltaPosition operator-(Position a, Position b) => new(a.X - b.X, a.Y - b.Y);
    
    public static Position operator+(Position a, DeltaPosition b) => new(a.X + b.X, a.Y + b.Y);
    
    public static implicit operator Position((int X, int Y) tuple) => new(tuple.X, tuple.Y);
}
public readonly record struct DeltaPosition(int X, int Y);