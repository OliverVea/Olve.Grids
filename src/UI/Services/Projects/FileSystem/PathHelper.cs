using Olve.Utilities.Projects;

namespace UI.Services.Projects.FileSystem;

public static class PathHelper
{
    private const string ProjectFileDelimiter = "_";
    private const string ProjectFileExtension = ".prj";
    private const string ProjectSummaryFileExtension = ".prjs";

    private const string ProjectName = "Grids";
    private const string ProjectsFolderName = "ui-projects";

    private static readonly ProjectFolderHelper ProjectFolderHelper = new(ProjectName);

    private static readonly ProjectFileNameHelper ProjectFileNameHelper =
        new(ProjectFileDelimiter, ProjectFileExtension);

    private static readonly ProjectFileNameHelper ProjectSummaryFileNameHelper =
        new(ProjectFileDelimiter, ProjectSummaryFileExtension);

    public static string ProjectsFolder => ProjectFolderHelper.GetSubfolder(ProjectsFolderName);

    public static string GetProjectFileName(Project project) =>
        ProjectFileNameHelper.GetFileName([ project.Id.ToString(), ]);

    public static string GetProjectPath(Project project) =>
        Path.Combine(ProjectsFolder, GetProjectFileName(project));

    public static IEnumerable<string> GetProjectFilePaths() =>
        ProjectFolderHelper.Search($"{ProjectsFolderName}/*{ProjectFileExtension}");

    public static string GetSummaryFileName(ProjectSummary projectSummary) =>
        ProjectSummaryFileNameHelper.GetFileName([ projectSummary.ProjectId.ToString(), ]);

    public static string GetSummaryPath(ProjectSummary projectSummary) =>
        Path.Combine(ProjectsFolder, GetSummaryFileName(projectSummary));

    public static IEnumerable<string> GetProjectSummaryFilePaths() =>
        ProjectFolderHelper.Search($"{ProjectsFolderName}/*{ProjectSummaryFileExtension}");
}