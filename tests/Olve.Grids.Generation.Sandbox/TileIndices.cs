using Olve.Grids.Grids;

namespace Olve.Grids.Generation.Sandbox;

public class TileIndices
{
    /// <summary>
    /// Light lower right
    /// </summary>
    public static readonly TileIndex I0 = new(0);
    
    /// <summary>
    /// Light lower / Dark upper
    /// </summary>
    public static readonly TileIndex I1 = new(1);
    
    /// <summary>
    /// Light lower left
    /// </summary>
    public static readonly TileIndex I2 = new(2);
    
    /// <summary>
    /// Dark upper left
    /// </summary>
    public static readonly TileIndex I3 = new(3);
    
    /// <summary>
    /// Dark upper right
    /// </summary>
    public static readonly TileIndex I4 = new(4);
    
    /// <summary>
    /// Light left / Dark right
    /// </summary>
    public static readonly TileIndex I5 = new(5);
    
    /// <summary>
    /// Full light
    /// </summary>
    public static readonly TileIndex I6 = new(6);
    
    /// <summary>
    /// Light right / Dark left
    /// </summary>
    public static readonly TileIndex I7 = new(7);
    
    /// <summary>
    /// Dark lower right
    /// </summary>
    public static readonly TileIndex I8 = new(8);
    
    /// <summary>
    /// Dark lower left
    /// </summary>
    public static readonly TileIndex I9 = new(9);
    
    /// <summary>
    /// Light upper right
    /// </summary>
    public static readonly TileIndex I10 = new(10);
    
    /// <summary>
    /// Light upper / Dark lower
    /// </summary>
    public static readonly TileIndex I11 = new(11);
    
    /// <summary>
    /// Light upper left
    /// </summary>
    public static readonly TileIndex I12 = new(12);
    
    /// <summary>
    /// Full dark
    /// </summary>
    public static readonly TileIndex I13 = new(13);
    
    /// <summary>
    /// Fallback tile
    /// </summary>
    public static readonly TileIndex I14 = new(14); 
}