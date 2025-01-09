using Olve.Utilities.Projects;

namespace UI.Core.Services.Projects.FileSystem;

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

    public static string GetProjectFileName(Id<Project> projectId) =>
        ProjectFileNameHelper.GetFileName([ projectId.ToString(), ]);

    public static string GetProjectPath(Id<Project> projectId) =>
        Path.Combine(ProjectsFolder, GetProjectFileName(projectId));

    public static string GetProjectFileName(Project project) => GetProjectFileName(project.Id);
    public static string GetProjectPath(Project project) => GetProjectPath(project.Id);

    public static string GetProjectFileName(ProjectSummary projectSummary) => GetProjectFileName(projectSummary.ProjectId);
    public static string GetProjectPath(ProjectSummary projectSummary) => GetProjectPath(projectSummary.ProjectId);

    public static IEnumerable<string> GetProjectFilePaths() =>
        ProjectFolderHelper.Search($"{ProjectsFolderName}/*{ProjectFileExtension}");


    public static string GetSummaryFileName(Id<Project> projectId) =>
        ProjectSummaryFileNameHelper.GetFileName([ projectId.ToString(), ]);

    public static string GetSummaryPath(Id<Project> projectId) =>
        Path.Combine(ProjectsFolder, GetSummaryFileName(projectId));

    public static string GetSummaryFileName(ProjectSummary projectSummary) => GetSummaryFileName(projectSummary.ProjectId);

    public static string GetSummaryPath(ProjectSummary projectSummary) => GetSummaryPath(projectSummary.ProjectId);

    public static IEnumerable<string> GetProjectSummaryFilePaths(string? searchString = null) =>
        ProjectFolderHelper.Search($"{ProjectsFolderName}/{searchString ?? string.Empty}*{ProjectSummaryFileExtension}");

    public static string GetTileSheetPath(Id<Project> projectId, string extension) =>
        Path.Combine(ProjectsFolder, $"{projectId}{extension}");
}