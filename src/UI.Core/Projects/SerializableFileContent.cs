using MemoryPack;
using SixLabors.ImageSharp;

namespace UI.Core.Projects;

[MemoryPackable]
public partial class SerializableFileContent
{
    public required string Name { get; init; }

    public static SerializableFileContent FromFileContent(FileContent fileContent) =>
        new()
        {
            Name = fileContent.Name,
        };

    public FileContent ToFileContent(Image image) =>
        new(Name, image);
}