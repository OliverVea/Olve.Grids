namespace UI.Core.Projects;

public class ProjectCache(int capacity = 1)
{

    private readonly HashSet<Id<Project>> _projectIds = [ ];
    private readonly LinkedList<Project> _projects = [ ];

    public OneOf<Project, NotFound> GetProject(Id<Project> projectId)
    {
        if (_projects.Count == 0)
        {
            return new NotFound();
        }

        if (!_projectIds.Contains(projectId))
        {
            return new NotFound();
        }

        foreach (var project in _projects)
        {
            if (project.Id == projectId)
            {
                _projects.Remove(project);
                _projects.AddFirst(project);

                return project;
            }
        }

        return new NotFound();
    }

    public void AddProject(Project project)
    {
        if (!_projectIds.Add(project.Id))
        {
            _projects.Remove(project);
        }

        _projects.AddFirst(project);


        if (_projects.Count > capacity)
        {
            RemoveLast();
        }
    }

    private void RemoveLast()
    {
        if (_projects.Last is { } last)
        {
            _projectIds.Remove(last.Value.Id);
            _projects.Remove(last);
        }
    }
}