namespace UI.Services.Projects.FileSystem;

public class ProjectNameComparer : IComparer<ProjectSummary>
{
    public static readonly ProjectNameComparer Shared = new();
    
    public int Compare(ProjectSummary? x, ProjectSummary? y)
    {
        return string.Compare(x?.Name.Value, y?.Name.Value, StringComparison.InvariantCultureIgnoreCase);
    }
}