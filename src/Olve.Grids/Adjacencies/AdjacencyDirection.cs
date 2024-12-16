namespace Olve.Grids.Adjacencies;

[Flags]
public enum AdjacencyDirection : byte
{
    None = 0,
    Up = 1,
    Left = 2,
    Right = 4,
    Down = 8,


    All = Up | Left | Right | Down
}