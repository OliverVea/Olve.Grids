using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

public class FrozenAdjacencyLookupTests
{
    [Test]
    public async Task this_OnEmptyLookup_ReturnsNone()
    {
        // Arrange
        var lookup = new FrozenAdjacencyLookup();

        var (from, to) = GetTilePair();

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(Direction.None);
    }

    [Test]
    [MethodDataSource<AdjacencyDirectionGenerator>("GetDirections")]
    public async Task this_SetFromAndTo_ReturnsSetDirection(Direction direction)
    {
        // Arrange
        var lookup = new FrozenAdjacencyLookup();
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
    public async Task this_SetFromAndTo_ReturnsOppositeDirection(Direction direction, Direction opposite)
    {
        // Arrange
        var lookup = new FrozenAdjacencyLookup();
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
        var lookup = new FrozenAdjacencyLookup();
        var (from, to) = GetTilePair();
        lookup.Set(from, to, Direction.Up);
        lookup.Set(from, to, Direction.Down);

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(Direction.Down);
    }

    [Test]
    public async Task this_SetOnSameTile_DirectionAndOppositeIsWritten()
    {
        // Arrange
        var lookup = new FrozenAdjacencyLookup();
        var tile = new TileIndex(42);
        var direction = Direction.Up;
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
    private static readonly Direction[] AllDirections =
        Enumerable
            .Range(1, (int)Direction.All)
            .Select(x => (Direction)x)
            .ToArray();

    public static IEnumerable<Func<Direction>> GetDirections()
    {
        return AllDirections.Select<Direction, Func<Direction>>(direction => () => direction);
    }

    public static IEnumerable<Func<(Direction direction, Direction opposite)>>
        GetDirectionsWithOpposites()
    {
        return AllDirections.Select<Direction, Func<(Direction direction, Direction opposite)>>(
            direction => () => (direction, direction.Opposite()));
    }
}