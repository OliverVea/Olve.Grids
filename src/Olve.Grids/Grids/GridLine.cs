using System.Runtime.InteropServices;

namespace Olve.Grids.Grids;

[StructLayout(LayoutKind.Sequential)]
public readonly record struct GridLine(GridPoint From, GridPoint To);