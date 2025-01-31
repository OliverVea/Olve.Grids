using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.Primitives;
using Olve.Grids.Serialization.Models;
using SixLabors.ImageSharp;

namespace UI.Core.Projects;

[MemoryPackable]
public partial class SerializableProject
{
    public const string SerializationVersion = "0.0.14-alpha";

    public string Version { get; init; } = SerializationVersion;
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required long CreatedAt { get; init; }
    public required long LastAccessedAt { get; init; }
    public required SerializableGridConfiguration GridConfiguration { get; init; }
    public required SerializableFileContent TileSheetImage { get; init; }
    public required SerializableAdjacencyLookup AdjacencyLookup { get; init; }
    public required SerializableBrushLookup BrushLookup { get; init; }
    public required SerializableWeightLookup WeightLookup { get; init; }
    public required int[] ActiveTileIds { get; init; }
    public required (int, int)[] LockedSides { get; init; }
    public required SerializableProjectBrush[] Brushes { get; init; }
    public string ImageName { get; init; } = string.Empty;

    public static (SerializableProject Project, Image Image) FromProject(Project project) =>
        (new SerializableProject
        {
            Id = project.Id.ToString(),
            Name = project.Name.Value,
            CreatedAt = project.CreatedAt.ToUnixTimeSeconds(),
            LastAccessedAt = project.CreatedAt.ToUnixTimeSeconds(),
            TileSheetImage = SerializableFileContent.FromFileContent(project.TileSheetImage),
            GridConfiguration = SerializableGridConfiguration.FromGridConfiguration(project.GridConfiguration),
            AdjacencyLookup = SerializableAdjacencyLookup.FromAdjacencyLookup(project.AdjacencyLookup),
            BrushLookup = SerializableBrushLookup.FromBrushLookup(project.BrushLookup),
            WeightLookup = SerializableWeightLookup.FromWeightLookup(project.WeightLookup),
            ImageName = project.TileSheetImage.Name,
            ActiveTileIds = project
                .ActiveTiles
                .Select(x => x.Index)
                .ToArray(),
            LockedSides = project
                .LockedSides.Select(x => (x.Item1.Index, (int)x.Item2))
                .ToArray(),
            Brushes = project
                .Brushes
                .Select(x => SerializableProjectBrush.FromProjectBrush(x.Value))
                .ToArray(),
        }, project.TileSheetImage.Image);


    public Project ToProject(Image tileSheetImage)
    {
        if (Version != SerializationVersion)
        {
            throw new InvalidOperationException($"Cannot deserialize project with version {Version}");
        }

        return new Project
        {
            Id = Id<Project>.Parse(Id),
            Name = new ProjectName(Name),
            CreatedAt = DateTimeOffset.FromUnixTimeSeconds(CreatedAt),
            LastAccessedAt = DateTimeOffset.FromUnixTimeSeconds(LastAccessedAt),
            TileSheetImage = TileSheetImage.ToFileContent(tileSheetImage),
            GridConfiguration = GridConfiguration.ToGridConfiguration(),
            AdjacencyLookup = AdjacencyLookup.ToAdjacencyLookup(),
            BrushLookup = BrushLookup.ToBrushLookup(),
            WeightLookup = WeightLookup.ToWeightLookup(),
            ActiveTiles = ActiveTileIds
                .Select(x => new TileIndex(x))
                .ToHashSet(),
            Brushes = Brushes.ToDictionary(x => new BrushId(x.Id),
                x => x.ToProjectBrush()),
            LockedSides = LockedSides
                .Select(x => (new TileIndex(x.Item1), (Side)x.Item2))
                .ToHashSet(),
        };
    }
}