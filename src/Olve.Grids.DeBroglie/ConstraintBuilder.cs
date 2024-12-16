using DeBroglie.Constraints;
using Olve.Grids.Generation;

namespace Olve.Grids.DeBroglie;

public class TileAtlasConstraintBuilder
{
    public IEnumerable<ITileConstraint> BuildConstraints(TileAtlas tileAtlas, BrushGrid brushGrid)
    {
        yield return new BrushConstraint(tileAtlas, brushGrid);
    }
}