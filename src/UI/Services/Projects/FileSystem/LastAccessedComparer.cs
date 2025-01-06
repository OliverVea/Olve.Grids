namespace UI.Services.Projects.FileSystem;

public class LastAccessedComparer : IComparer<ProjectSummary>
{
    public static readonly LastAccessedComparer Shared = new();
    
    public int Compare(ProjectSummary? x, ProjectSummary? y)
    {
        return DateTimeOffset.Compare(x?.LastAccessed ?? DateTimeOffset.MinValue, y?.LastAccessed ?? DateTimeOffset.MinValue);
    }
}