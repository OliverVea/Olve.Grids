using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.TileAtlasBuilder;
using Olve.Grids.Serialization.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;

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
    public required SerializedFileContent TileSheetImage { get; init; }
    public required SerializableAdjacencyLookup AdjacencyLookup { get; init; }
    public required SerializableBrushLookup BrushLookup { get; init; }
    public required SerializableWeightLookup WeightLookup { get; init; }
    public required int[] ActiveTileIds { get; init; }
    public required SerializedProjectBrush[] Brushes { get; init; }
    public string ImageName { get; init; } = string.Empty;

    public static (SerializableProject Project, Image Image) FromProject(Project project) =>
        (new SerializableProject
        {
            Id = project.Id.ToString(),
            Name = project.Name.Value,
            CreatedAt = project.CreatedAt.ToUnixTimeSeconds(),
            LastAccessedAt = project.CreatedAt.ToUnixTimeSeconds(),
            TileSheetImage = SerializedFileContent.FromFileContent(project.TileSheetImage),
            GridConfiguration = SerializableGridConfiguration.FromGridConfiguration(project.GridConfiguration),
            AdjacencyLookup = SerializableAdjacencyLookup.FromAdjacencyLookup(project.AdjacencyLookup),
            BrushLookup = SerializableBrushLookup.FromBrushLookup(project.BrushLookup),
            WeightLookup = SerializableWeightLookup.FromWeightLookup(project.WeightLookup),
            ImageName = project.TileSheetImage.Name,
            ActiveTileIds = project
                .ActiveTiles
                .Select(x => x.Index)
                .ToArray(),
            Brushes = project
                .Brushes
                .Select(x => SerializedProjectBrush.FromProjectBrush(x.Value))
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
        };
    }
}

[MemoryPackable]
public partial class SerializedProjectBrush
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Color { get; init; }

    public static SerializedProjectBrush FromProjectBrush(ProjectBrush projectBrush) =>
        new()
        {
            Id = projectBrush.Id.Value,
            DisplayName = projectBrush.DisplayName,
            Color = projectBrush.Color.Value,
        };

    public ProjectBrush ToProjectBrush() => new(new BrushId(Id), DisplayName, new ColorString(Color));
}

[MemoryPackable]
public partial class SerializedFileContent
{
    public required string Name { get; init; }

    public static SerializedFileContent FromFileContent(FileContent fileContent) =>
        new()
        {
            Name = fileContent.Name,
        };

    public FileContent ToFileContent(Image image) =>
        new(Name, image);
}