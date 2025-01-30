using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

[InheritsTests]
public class BrushLookupBrushLookupTests : BrushLookupTests<BrushLookup>
{
    protected override BrushLookup CreateLookup(
        IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)>? items = null) => new(items);
}

public abstract class BrushLookupTests<TBrushLookup>
    where TBrushLookup : IBrushLookup
{
    protected abstract TBrushLookup CreateLookup(
        IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)>? items = null);


    [Test]
    public async Task GetBrushId_AfterClearingTileBrush_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var corner = Corner.UpperRight;
        var brushId = new BrushId();

        var lookup = CreateLookup([ (tile, corner, brushId), ]);

        lookup.ClearTileBrush(tile, corner);

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert
            .That(actual.IsT1)
            .IsTrue();
    }

    [Test]
    public async Task GetBrushId_AfterClearingAllTileBrushes_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = new BrushId();

        var lookup = CreateLookup([ (tile, Corner.UpperRight, brushId), (tile, Corner.LowerLeft, brushId), ]);
        lookup.ClearTileBrushes(tile);

        // Act
        var actual1 = lookup.GetBrushId(tile, Corner.UpperRight);
        var actual2 = lookup.GetBrushId(tile, Corner.LowerLeft);

        // Assert
        await Assert
            .That(actual1.IsT1)
            .IsTrue();
        await Assert
            .That(actual2.IsT1)
            .IsTrue();
    }

    [Test]
    public async Task GetTiles_AfterClearingBrushIdForAllTiles_ReturnsNotFound()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;

        var lookup = CreateLookup([ (tileA, corner, brushId), (tileB, corner, brushId), ]);
        lookup.Clear();

        // Act
        var actual = lookup.GetTiles(brushId, corner);

        // Assert
        await Assert
            .That(actual.IsT1)
            .IsTrue();
    }


    [Test]
    public async Task Brushes_AfterMultipleClears_ProducesEmptyLookup()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;

        var lookup = CreateLookup([ (tile, corner, brushId), ]);

        lookup.Clear();
        lookup.SetCornerBrush(tile, corner, brushId);
        lookup.Clear();


        // Act
        var allBrushIds = lookup.Brushes;

        // Assert
        await Assert
            .That(allBrushIds)
            .IsEmpty();
    }
}