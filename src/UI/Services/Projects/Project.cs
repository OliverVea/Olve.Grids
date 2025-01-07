using Olve.Utilities.Lookup;

namespace UI.Services.Projects;

public record Project(
    Id<Project> Id,
    ProjectName Name,
    DateTimeOffset CreatedAt,
    DateTimeOffset LastAccessedAt)
    : IHasId<Id<Project>>;