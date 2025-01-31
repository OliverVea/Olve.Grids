using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

[InheritsTests]
public class AdjacencyLookupReadOnlyAdjacencyLookupTests : ReadOnlyAdjacencyLookupTests<AdjacencyLookup>
{
    protected override AdjacencyLookup CreateLookup(
        IEnumerable<TileAdjacency>? values = null) => new(values);
}

[InheritsTests]
public class FrozenAdjacencyLookupReadOnlyAdjacencyLookupTests : ReadOnlyAdjacencyLookupTests<FrozenAdjacencyLookup>
{
    protected override FrozenAdjacencyLookup CreateLookup(
        IEnumerable<TileAdjacency>? values = null) => new(GetFrozenValues(values));

    private static IEnumerable<TileAdjacency> GetFrozenValues(
        IEnumerable<TileAdjacency>? values)
    {
        if (values is null)
        {
            yield break;
        }

        foreach (var (from, to, direction) in values)
        {
            if (from == to)
            {
                yield return (from, to, direction | direction.Opposite());
            }
            else
            {
                yield return (from, to, direction);
                yield return (to, from, direction.Opposite());
            }
        }
    }
}

public abstract class ReadOnlyAdjacencyLookupTests<TLookup>
    where TLookup : IReadOnlyAdjacencyLookup
{
    protected abstract TLookup CreateLookup(IEnumerable<TileAdjacency>? values =
        null);


    [Test]
    public async Task this_OnEmptyLookup_ReturnsNone()
    {
        // Arrange
        var lookup = CreateLookup();

        var (from, to) = TestHelper.GetTilePair();

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(Direction.None);
    }

    [Test]
    [MethodDataSource<TestHelper>(nameof(TestHelper.AllDirections))]
    public async Task this_SetFromAndTo_ReturnsSetDirection(Direction direction)
    {
        // Arrange
        var (from, to) = TestHelper.GetTilePair();
        var lookup = CreateLookup([ (from, to, direction), ]);

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(direction);
    }

    [Test]
    [MethodDataSource<TestHelper>(nameof(TestHelper.GetDirectionsWithOpposites))]
    public async Task this_SetFromAndTo_ReturnsOppositeDirection(Direction direction, Direction opposite)
    {
        // Arrange
        var (from, to) = TestHelper.GetTilePair();
        var lookup = CreateLookup([ (from, to, direction), ]);

        // Act
        var result = lookup.Get(to, from);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(opposite);
    }

    [Test]
    [MethodDataSource<TestHelper>(nameof(TestHelper.GetDirectionsWithOpposites))]
    public async Task this_SetOnSameTile_DirectionAndOppositeIsWritten(Direction direction, Direction opposite)
    {
        // Arrange
        var tile = new TileIndex(42);
        var lookup = CreateLookup([ (tile, tile, direction), ]);

        // Act
        var result = lookup.Get(tile, tile);

        // Assert
        await Assert
            .That(result)
            .IsEqualTo(direction | opposite);
    }
}