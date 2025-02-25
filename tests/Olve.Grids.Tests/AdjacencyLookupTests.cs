﻿using Olve.Grids.Adjacencies;
using Olve.Grids.Primitives;

namespace Olve.Grids.Tests;

[InheritsTests]
public class AdjacencyLookupAdjacencyLookupTests : AdjacencyLookupTests<AdjacencyLookup>
{
    protected override AdjacencyLookup CreateLookup(
        IEnumerable<TileAdjacency>? values = null) => new(values);
}

public abstract class AdjacencyLookupTests<TLookup>
    where TLookup : IAdjacencyLookup
{
    protected abstract TLookup CreateLookup(IEnumerable<TileAdjacency>? values =
        null);

    [Test]
    [MethodDataSource<TestHelper>(nameof(TestHelper.GetDirectionsWithOtherDirections))]
    public async Task this_SetAndSetAgain_OverwritesDirection(Direction direction, Direction otherDirection)
    {
        // Arrange
        var (from, to) = TestHelper.GetTilePair();

        var lookup = CreateLookup([ (from, to, direction), ]);

        lookup.Set((from, to, otherDirection));

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