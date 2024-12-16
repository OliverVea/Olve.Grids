using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;

namespace Olve.Grids.Tests;

public class AdjacencyLookupTests
{
    [Test]
    public async Task this_OnEmptyLookup_ReturnsNone()
    {
        // Arrange
        var lookup = new AdjacencyLookup();

        var (from, to) = GetTilePair();

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(AdjacencyDirection.None);
    }

    [Test]
    [MethodDataSource<AdjacencyDirectionGenerator>("GetDirections")]
    public async Task this_SetFromAndTo_ReturnsSetDirection(AdjacencyDirection direction)
    {
        // Arrange
        var lookup = new AdjacencyLookup();
        var (from, to) = GetTilePair();
        lookup.Set(from, to, direction);

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(direction);
    }

    [Test]
    [MethodDataSource<AdjacencyDirectionGenerator>("GetDirectionsWithOpposites")]
    public async Task this_SetFromAndTo_ReturnsOppositeDirection(AdjacencyDirection direction, AdjacencyDirection opposite)
    {
        // Arrange
        var lookup = new AdjacencyLookup();
        var (from, to) = GetTilePair();
        lookup.Set(from, to, direction);

        // Act
        var result = lookup.Get(to, from);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(opposite);
    }

    [Test]
    public async Task this_SetAndSetAgain_OverwritesDirection()
    {
        // Arrange
        var lookup = new AdjacencyLookup();
        var (from, to) = GetTilePair();
        lookup.Set(from, to, AdjacencyDirection.Up);
        lookup.Set(from, to, AdjacencyDirection.Down);

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(AdjacencyDirection.Down);
    }

    [Test]
    public async Task this_SetOnSameTile_DirectionAndOppositeIsWritten()
    {
        // Arrange
        var lookup = new AdjacencyLookup();
        var tile = new TileIndex(42);
        var direction = AdjacencyDirection.Up;
        var opposite = direction.Opposite();

        lookup.Set(tile, tile, direction);

        // Act
        var result = lookup.Get(tile, tile);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(direction | opposite);
    }


    private static (TileIndex from, TileIndex to) GetTilePair()
    {
        var from = new TileIndex(0);
        var to = new TileIndex(1);

        return (from, to);
    }
}

public class AdjacencyDirectionGenerator
{
    private static readonly AdjacencyDirection[] AllDirections =
        Enumerable
            .Range(1, (int)AdjacencyDirection.All)
            .Select(x => (AdjacencyDirection)x)
            .ToArray();

    public static IEnumerable<Func<AdjacencyDirection>> GetDirections()
    {
        return AllDirections.Select<AdjacencyDirection, Func<AdjacencyDirection>>(direction => () => direction);
    }

    public static IEnumerable<Func<(AdjacencyDirection direction, AdjacencyDirection opposite)>>
        GetDirectionsWithOpposites()
    {
        return AllDirections.Select<AdjacencyDirection, Func<(AdjacencyDirection direction, AdjacencyDirection opposite)>>(
            direction => () => (direction, direction.Opposite()));
    }
}