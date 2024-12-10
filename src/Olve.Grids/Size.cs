namespace Olve.Grids;

public readonly record struct Size(int Width, int Height)
{
    public static implicit operator Size((int Width, int Height) tuple) => new(tuple.Width, tuple.Height);
}