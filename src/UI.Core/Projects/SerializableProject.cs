using MemoryPack;
using Olve.Grids.Brushes;
using Olve.Grids.Grids;
using Olve.Grids.IO.TileAtlasBuilder;

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
    public required SerializedFileContent TileSheetImage { get; init; }
    public required SerializableTileAtlasConfiguration TileAtlasBuilder { get; init; }
    public int[]? ActiveTileIds { get; init; }
    public SerializedProjectBrush[]? Brushes { get; init; }

    public static (SerializableProject Project, byte[] ImageBytes) FromProject(Project project) =>
        (new SerializableProject
        {
            Id = project.Id.ToString(),
            Name = project.Name.Value,
            CreatedAt = project.CreatedAt.ToUnixTimeSeconds(),
            LastAccessedAt = project.CreatedAt.ToUnixTimeSeconds(),
            TileSheetImage = SerializedFileContent.FromFileContent(project.TileSheetImage),
            TileAtlasBuilder =
                SerializableTileAtlasConfiguration.FromTileAtlasConfiguration(project.TileAtlasBuilder.Configuration),
            ActiveTileIds = project
                .ActiveTiles.Select(x => x.Index)
                .ToArray(),
            Brushes = project
                .Brushes.Values
                .Select(SerializedProjectBrush.FromProjectBrush)
                .ToArray(),
        }, project.TileSheetImage.Content);


    public Project ToProject(byte[] tileSheetContent)
    {
        if (Version != SerializationVersion)
        {
            throw new InvalidOperationException($"Cannot deserialize project with version {Version}");
        }

        return new Project(
            Id<Project>.Parse(Id),
            new ProjectName(Name),
            DateTimeOffset.FromUnixTimeSeconds(CreatedAt),
            DateTimeOffset.FromUnixTimeSeconds(LastAccessedAt),
            TileSheetImage.ToFileContent(tileSheetContent),
            ActiveTileIds
                ?
                .Select(x => new TileIndex(x))
                .ToHashSet()
            ?? [ ],
            Brushes
                ?
                .Select(x => x.ToProjectBrush())
                .ToDictionary(x => x.Id)
            ?? [ ],
            new TileAtlasBuilder(TileAtlasBuilder.ToTileAtlasConfiguration()));
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
    public required int Width { get; init; }
    public required int Height { get; init; }

    public static SerializedFileContent FromFileContent(FileContent fileContent) =>
        new()
        {
            Name = fileContent.Name,
            Width = fileContent.Size.Width,
            Height = fileContent.Size.Height,
        };

    public FileContent ToFileContent(byte[] content) =>
        new(Name, content, new Size(Width, Height));
}