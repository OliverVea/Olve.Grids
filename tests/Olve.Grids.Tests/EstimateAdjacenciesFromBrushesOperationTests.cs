using System.Diagnostics.CodeAnalysis;
using Olve.Grids.Adjacencies;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Request = Olve.Grids.Adjacencies.EstimateAdjacenciesFromBrushesOperation.Request;
using static Olve.Grids.Tests.EstimateAdjacenciesFromBrushesOperationTests.BrushPatterns;

namespace Olve.Grids.Tests;

public class EstimateAdjacenciesFromBrushesOperationTests
{
    private static EstimateAdjacenciesFromBrushesOperation Operation => new();

    [Test]
    public async Task Execute_WithValidRequest_ShouldReturnSuccessResult()
    {
        // Arrange
        AdjacencyLookup adjacencyLookup = new();
        BrushLookup brushLookup = new();

        Request request = new(adjacencyLookup, brushLookup.TileBrushes);

        // Act
        var result = Operation.Execute(request);

        // Assert
        await Assert
            .That(result.Succeeded)
            .IsTrue();
    }

    [Test]
    [MethodDataSource<TestCases>(nameof(TestCases.GetEstimateAdjacenciesCases))]
    public async Task Execute_WithValidRequest_ShouldEstimateAdjacencies(string aBrushes,
        string bBrushes,
        Direction expected)
    {
        // Arrange
        var (a, b) = TestHelper.GetTilePair();

        AdjacencyLookup adjacencyLookup = new();
        BrushLookup brushLookup = new([ (a, GetCornerBrushes(aBrushes)), (b, GetCornerBrushes(bBrushes)), ]);

        Request request = new(adjacencyLookup, brushLookup.TileBrushes);

        // Act
        var result = Operation.Execute(request);
        var adjacency = adjacencyLookup.Get(a, b);

        // Assert
        await Assert
            .That(result.Succeeded)
            .IsTrue();
        await Assert
            .That(adjacency)
            .IsEqualTo(expected);
    }

    [Test]
    [Arguments(Direction.Right, Side.Right, true)]
    [Arguments(Direction.Right, Side.Left, false)]
    [Arguments(Direction.All, Side.Right, true)]
    [Arguments(Direction.All, Side.Left, false)]
    public async Task Execute_WithLockedAdjacencies_LockedAdjacenciesAreNotUpdated(Direction existingDirection,
        Side lockedSide,
        bool hasRight)
    {
        // Arrange
        string aBrushes = AAAA, bBrushes = BBBB;

        var (a, b) = TestHelper.GetTilePair();

        AdjacencyLookup adjacencyLookup = new([ (a, b, Direction.All), ]);
        BrushLookup brushLookup = new([ (a, GetCornerBrushes(aBrushes)), (b, GetCornerBrushes(bBrushes)), ]);

        Request request = new(adjacencyLookup, brushLookup.TileBrushes)
        {
            ToNotUpdate = new HashSet<(TileIndex, Side)>
            {
                (a, lockedSide),
            },
        };

        // Act
        var result = Operation.Execute(request);

        var adjacency = adjacencyLookup.Get(a, b);

        // Assert
        await Assert
            .That(result.Succeeded)
            .IsTrue();

        await Assert
            .That(adjacency.HasFlag(Direction.Right))
            .IsEqualTo(hasRight);
    }

    [Test]
    [Arguments(Direction.None, null, true)]
    [Arguments(Direction.Right, null, true)]
    [Arguments(Direction.None, Side.Right, false)]
    [Arguments(Direction.None, Side.Left, true)]
    [Arguments(Direction.Right, Side.Right, true)]
    [Arguments(Direction.All, Side.Right, true)]
    public async Task Execute_OnTilesWithSameBrushesAndLockedAdjacencies_LockedAdjacenciesAreNotUpdated(
        Direction existingDirection,
        Side? lockedSide,
        bool hasRight)
    {
        // Arrange
        string aBrushes = AAAA, bBrushes = AAAA;

        var (a, b) = TestHelper.GetTilePair();

        AdjacencyLookup adjacencyLookup = new([ (a, b, existingDirection), ]);
        BrushLookup brushLookup = new([ (a, GetCornerBrushes(aBrushes)), (b, GetCornerBrushes(bBrushes)), ]);

        Request request = new(adjacencyLookup, brushLookup.TileBrushes);
        if (lockedSide.HasValue)
        {
            request.ToNotUpdate = new HashSet<(TileIndex, Side)>
            {
                (a, lockedSide.Value),
            };
        }

        // Act
        var result = Operation.Execute(request);

        var adjacency = adjacencyLookup.Get(a, b);

        // Assert
        await Assert
            .That(result.Succeeded)
            .IsTrue();

        await Assert
            .That(adjacency.HasFlag(Direction.Right))
            .IsEqualTo(hasRight);
    }

    public class TestCases
    {
        public static IEnumerable<(string A, string B, Direction Expected)> GetEstimateAdjacenciesCases()
        {
            yield return (AAAA, AAAA, Direction.All);
            yield return (AAAA, BBBB, Direction.None);
            yield return (ABAB, BABA, Direction.X);
            yield return (ABAB, ABAB, Direction.Y);
            yield return (ABCB, CBCC, Direction.Down);
            yield return (ABCD, EFGH, Direction.None);
        }

    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "IdentifierTypo")]
    public static class BrushPatterns
    {
        public const string AAAA = "aa aa";
        public const string ABAB = "ab ab";
        public const string BABA = "ba ba";
        public const string BBBB = "bb bb";
        public const string ABCB = "ab cb";
        public const string CBCC = "cb cc";
        public const string ABCD = "ab cd";
        public const string EFGH = "ef gh";
    }

    private static CornerBrushes GetCornerBrushes(string tileBrushes)
    {
        if (tileBrushes is not [ _, _, ' ', _, _, ])
        {
            throw new ArgumentException($"Invalid tile brushes '{tileBrushes}'");
        }

        var topLeft = tileBrushes[0];
        var topRight = tileBrushes[1];
        var bottomLeft = tileBrushes[3];
        var bottomRight = tileBrushes[4];

        return new CornerBrushes
        {
            UpperLeft = new BrushId(topLeft.ToString()),
            UpperRight = new BrushId(topRight.ToString()),
            LowerLeft = new BrushId(bottomLeft.ToString()),
            LowerRight = new BrushId(bottomRight.ToString()),
        };
    }
}