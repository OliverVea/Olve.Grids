using MemoryPack;
using Olve.Grids.IO.TileAtlasBuilder;

namespace UI.Services.Projects;

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
            new TileAtlasBuilder(TileAtlasBuilder.ToTileAtlasConfiguration()));
    }
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

[MemoryPackable]
public partial class SerializedTileAtlasBuilder
{
    public static SerializedTileAtlasBuilder FromTileAtlasBuilder(TileAtlasBuilder tileAtlasBuilder) =>
        new();

    public TileAtlasBuilder ToTileAtlasBuilder() =>
        new();
}