﻿namespace Olve.Grids.Primitives;

[Flags]
public enum Direction : byte
{
    None = 0,
    Up = 1,
    Left = 2,
    Right = 4,
    Down = 8,


    All = Up | Left | Right | Down,
}