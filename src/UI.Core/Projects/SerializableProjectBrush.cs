using MemoryPack;
using Olve.Grids.Brushes;

namespace UI.Core.Projects;

[MemoryPackable]
public partial class SerializableProjectBrush
{
    public required string Id { get; init; }
    public required string DisplayName { get; init; }
    public required string Color { get; init; }

    public static SerializableProjectBrush FromProjectBrush(ProjectBrush projectBrush) =>
        new()
        {
            Id = projectBrush.Id.Value,
            DisplayName = projectBrush.DisplayName,
            Color = projectBrush.Color.Value,
        };

    public ProjectBrush ToProjectBrush() => new(new BrushId(Id), DisplayName, new ColorString(Color));
}