using Olve.Grids.Brushes;
using Olve.Grids.Grids;

namespace Olve.Grids.Tests;

public class BrushLookupTests
{
    [Test]
    public async Task GetBrushId_NoBrushIdForTile_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var corner = Corner.UpperRight;

        var builder = new BrushLookupBuilder();
        var lookup = builder.Build();

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert.That(actual.IsT1).IsTrue();
    }

    [Test]
    public async Task GetBrushId_AfterSettingBrushIdOnTile_CorrectBrushIdIsReturned()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;

        var builder = new BrushLookupBuilder();

        builder.SetCornerBrush(tile, corner, brushId);

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert.That(actual.IsT0).IsTrue();
        await Assert.That(actual.AsT0).IsEqualTo(brushId);
    }

    [Test]
    public async Task GetTiles_AfterSettingBrushIdOnTileInOppositeDirection_TileIsReturned()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;
        var oppositeCorner = corner.Opposite();

        var builder = new BrushLookupBuilder();

        builder.SetCornerBrush(tile, corner, brushId);

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetTiles(brushId, oppositeCorner);

        // Assert
        await Assert.That(actual.IsT0).IsTrue();
        await Assert.That(actual.AsT0).Contains(tile);
    }

    [Test]
    public async Task GetTiles_WithMultipleTilesForBrushInDirection_ContainsBothTiles()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;
        var oppositeCorner = corner.Opposite();

        var builder = new BrushLookupBuilder();

        builder.SetCornerBrush(tileA, corner, brushId);
        builder.SetCornerBrush(tileB, corner, brushId);

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetTiles(brushId, oppositeCorner);

        // Assert
        await Assert.That(actual.IsT0).IsTrue();
        await Assert.That(actual.AsT0).Contains(tileA);
        await Assert.That(actual.AsT0).Contains(tileB);
    }

    [Test]
    public async Task GetTiles_AfterSettingBrushIdOnTileInWrongDirection_TileIsNot()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;
        var wrongCorner = Corner.LowerRight;

        var builder = new BrushLookupBuilder();

        builder.SetCornerBrush(tile, corner, brushId);

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetTiles(brushId, wrongCorner);

        // Assert
        await Assert.That(actual.IsT1).IsTrue();
    }

    [Test]
    public async Task GetBrushId_AfterClearingTileBrush_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var corner = Corner.UpperRight;
        var brushId = BrushId.New();

        var builder = new BrushLookupBuilder();
        builder.SetCornerBrush(tile, corner, brushId);
        builder.ClearTileBrush(tile, corner);

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetBrushId(tile, corner);

        // Assert
        await Assert.That(actual.IsT1).IsTrue();
    }

    [Test]
    public async Task GetBrushId_AfterClearingAllTileBrushes_ReturnsNotFound()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = BrushId.New();

        var builder = new BrushLookupBuilder();
        builder.SetCornerBrush(tile, Corner.UpperRight, brushId);
        builder.SetCornerBrush(tile, Corner.LowerLeft, brushId);
        builder.ClearTileBrushes(tile);

        var lookup = builder.Build();

        // Act
        var actual1 = lookup.GetBrushId(tile, Corner.UpperRight);
        var actual2 = lookup.GetBrushId(tile, Corner.LowerLeft);

        // Assert
        await Assert.That(actual1.IsT1).IsTrue();
        await Assert.That(actual2.IsT1).IsTrue();
    }

    [Test]
    public async Task GetTiles_AfterClearingBrushIdForAllTiles_ReturnsNotFound()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;

        var builder = new BrushLookupBuilder();
        builder.SetCornerBrush(tileA, corner, brushId);
        builder.SetCornerBrush(tileB, corner, brushId);
        builder.Clear();

        var lookup = builder.Build();

        // Act
        var actual = lookup.GetTiles(brushId, corner);

        // Assert
        await Assert.That(actual.IsT1).IsTrue();
    }

    [Test]
    public async Task GetTiles_AfterSettingSameBrushIdForDifferentCorners_TilesAreCorrectlyGrouped()
    {
        // Arrange
        var tileA = new TileIndex(42);
        var tileB = new TileIndex(43);
        var brushId = BrushId.New();

        var builder = new BrushLookupBuilder();
        builder.SetCornerBrush(tileA, Corner.UpperRight, brushId);
        builder.SetCornerBrush(tileB, Corner.LowerLeft, brushId);

        var lookup = builder.Build();

        // Act
        var actualUpperRight = lookup.GetTiles(brushId, Corner.UpperRight.Opposite());
        var actualLowerLeft = lookup.GetTiles(brushId, Corner.LowerLeft.Opposite());

        // Assert
        await Assert.That(actualUpperRight.IsT0).IsTrue();
        await Assert.That(actualUpperRight.AsT0).Contains(tileA);
        await Assert.That(actualUpperRight.AsT0).DoesNotContain(tileB);

        await Assert.That(actualLowerLeft.IsT0).IsTrue();
        await Assert.That(actualLowerLeft.AsT0).Contains(tileB);
        await Assert.That(actualLowerLeft.AsT0).DoesNotContain(tileA);
    }

    [Test]
    public async Task Build_WithNoBrushes_ReturnsEmptyLookup()
    {
        // Arrange
        var builder = new BrushLookupBuilder();

        var lookup = builder.Build();

        // Act
        var allBrushIds = lookup.Brushes;

        // Assert
        await Assert.That(allBrushIds).IsEmpty();
    }

    [Test]
    public async Task Build_AfterMultipleClears_ProducesEmptyLookup()
    {
        // Arrange
        var tile = new TileIndex(42);
        var brushId = BrushId.New();
        var corner = Corner.UpperRight;

        var builder = new BrushLookupBuilder();
        builder.SetCornerBrush(tile, corner, brushId);
        builder.Clear();
        builder.SetCornerBrush(tile, corner, brushId);
        builder.Clear();

        var lookup = builder.Build();

        // Act
        var allBrushIds = lookup.Brushes;

        // Assert
        await Assert.That(allBrushIds).IsEmpty();
    }
}