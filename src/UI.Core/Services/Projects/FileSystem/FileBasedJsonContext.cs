using System.Text.Json.Serialization;

namespace UI.Core.Services.Projects.FileSystem;

[JsonSerializable(typeof(Project))]
[JsonSerializable(typeof(ProjectSummary))]
public partial class FileBasedJsonContext : JsonSerializerContext;