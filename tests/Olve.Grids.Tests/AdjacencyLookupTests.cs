using Olve.Grids.Adjacencies;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

[InheritsTests]
public class AdjacencyLookupAdjacencyLookupTests : AdjacencyLookupTests<AdjacencyLookup>
{
    protected override AdjacencyLookup CreateLookup(
        IEnumerable<(TileIndex from, TileIndex to, Direction direction)>? values = null) => new(values);
}

public abstract class AdjacencyLookupTests<TLookup>
    where TLookup : IAdjacencyLookup
{
    protected abstract TLookup CreateLookup(IEnumerable<(TileIndex from, TileIndex to, Direction direction)>? values =
        null);

    [Test]
    [MethodDataSource<AdjacencyLookupTestHelper>(nameof(AdjacencyLookupTestHelper.GetDirectionsWithOtherDirections))]
    public async Task this_SetAndSetAgain_OverwritesDirection(Direction direction, Direction otherDirection)
    {
        // Arrange
        var (from, to) = AdjacencyLookupTestHelper.GetTilePair();

        var lookup = CreateLookup([ (from, to, direction), ]);

        lookup.Set(from, to, otherDirection);

        // Act
        var result = lookup.Get(from, to);

        // Assert
        await Assert
            .That(result)
            .IsNotEqualTo(direction);

        await Assert
            .That(result)
            .IsEqualTo(otherDirection);
    }
}