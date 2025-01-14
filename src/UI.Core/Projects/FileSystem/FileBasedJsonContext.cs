using System.Text.Json.Serialization;

namespace UI.Core.Projects.FileSystem;

[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(ProjectSummary))]
public partial class FileBasedJsonContext : JsonSerializerContext;