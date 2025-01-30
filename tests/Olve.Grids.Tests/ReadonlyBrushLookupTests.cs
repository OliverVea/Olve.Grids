using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

[InheritsTests]
public class BrushLookupReadOnlyBrushLookupTests : ReadonlyBrushLookupTests<BrushLookup>
{
    protected override BrushLookup CreateLookup(
        IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)>? items = null) => new(items);
}

[InheritsTests]
public class FrozenBrushLookupReadOnlyBrushLookupTests : ReadonlyBrushLookupTests<FrozenBrushLookup>
{
    protected override FrozenBrushLookup CreateLookup(
        IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)>? items = null) => new(items ?? [ ]);
}

public abstract class ReadonlyBrushLookupTests<TReadOnlyBrushLookup>
    where TReadOnlyBrushLookup : IReadOnlyBrushLookup
{
    protected abstract TReadOnlyBrushLookup CreateLookup(
        IEnumerable<(TileIndex TileIndex, Corner Corner, BrushId BrushId)>? items = null);


    [Test]
    public async Task GetBrushId_NoBrushIdForTile_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var corner = Corner.UpperRight;

        var lookup = CreateLookup();

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert
            .That(actual.IsT1)
            .IsTrue();
    }


    [Test]
    public async Task GetBrushId_AfterSettingBrushIdOnTile_CorrectBrushIdIsReturned()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;

        var lookup = CreateLookup([ (tile, corner, brushId), ]);

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert
            .That(actual.IsT0)
            .IsTrue();
        await Assert
            .That(actual.AsT0)
            .IsEqualTo(brushId);
    }


    [Test]
    public async Task GetTiles_AfterSettingBrushIdOnTileInOppositeDirection_TileIsReturned()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;
        var oppositeCorner = corner.Opposite();

        var lookup = CreateLookup([ (tile, corner, brushId), ]);


        // Act
        var actual = lookup.GetTiles(brushId, oppositeCorner);

        // Assert
        await Assert
            .That(actual.IsT0)
            .IsTrue();
        await Assert
            .That(actual.AsT0)
            .Contains(tile);
    }

    [Test]
    public async Task GetTiles_WithMultipleTilesForBrushInDirection_ContainsBothTiles()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;
        var oppositeCorner = corner.Opposite();

        var lookup = CreateLookup([ (tileA, corner, brushId), (tileB, corner, brushId), ]);

        // Act
        var actual = lookup.GetTiles(brushId, oppositeCorner);

        // Assert
        await Assert
            .That(actual.IsT0)
            .IsTrue();
        await Assert
            .That(actual.AsT0)
            .Contains(tileA);
        await Assert
            .That(actual.AsT0)
            .Contains(tileB);
    }

    [Test]
    public async Task GetTiles_AfterSettingBrushIdOnTileInWrongDirection_TileIsNot()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = new BrushId();
        var corner = Corner.UpperRight;
        var wrongCorner = Corner.LowerRight;

        var lookup = CreateLookup([ (tile, corner, brushId), ]);

        // Act
        var actual = lookup.GetTiles(brushId, wrongCorner);

        // Assert
        await Assert
            .That(actual.IsT1)
            .IsTrue();
    }


    [Test]
    public async Task GetTiles_AfterSettingSameBrushIdForDifferentCorners_TilesAreCorrectlyGrouped()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = new BrushId();

        var lookup = CreateLookup([ (tileA, Corner.UpperRight, brushId), (tileB, Corner.LowerLeft, brushId), ]);

        // Act
        var actualUpperRight = lookup.GetTiles(brushId, Corner.UpperRight.Opposite());
        var actualLowerLeft = lookup.GetTiles(brushId, Corner.LowerLeft.Opposite());

        // Assert
        await Assert
            .That(actualUpperRight.IsT0)
            .IsTrue();
        await Assert
            .That(actualUpperRight.AsT0)
            .Contains(tileA);
        await Assert
            .That(actualUpperRight.AsT0)
            .DoesNotContain(tileB);

        await Assert
            .That(actualLowerLeft.IsT0)
            .IsTrue();
        await Assert
            .That(actualLowerLeft.AsT0)
            .Contains(tileB);
        await Assert
            .That(actualLowerLeft.AsT0)
            .DoesNotContain(tileA);
    }

    [Test]
    public async Task Brushes_WithNoBrushes_ReturnsEmptyLookup()
    {
        // Arrange
        var lookup = CreateLookup();

        // Act
        var allBrushIds = lookup.Brushes;

        // Assert
        await Assert
            .That(allBrushIds)
            .IsEmpty();
    }

}