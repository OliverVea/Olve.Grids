using Olve.Utilities.Lookup;

namespace UI.Services.Projects;

public record Project(Id<Project> Id, ProjectName Name) : IHasId<Id<Project>>;