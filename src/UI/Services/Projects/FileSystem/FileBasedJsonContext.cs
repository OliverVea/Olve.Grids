using System.Text.Json.Serialization;

namespace UI.Services.Projects.FileSystem;

[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(ProjectSummary))]
public partial class FileBasedJsonContext : JsonSerializerContext;