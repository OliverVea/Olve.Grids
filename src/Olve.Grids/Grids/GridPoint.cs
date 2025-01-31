using System.Runtime.InteropServices;

namespace Olve.Grids.Grids;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct GridPoint(float X, float Y);