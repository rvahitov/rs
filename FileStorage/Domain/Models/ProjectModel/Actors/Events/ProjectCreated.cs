namespace Domain.Models.ProjectModel.Actors.Events
{
    internal sealed class ProjectCreated : IProjectEvent
    {
        public ProjectCreated(ProjectName projectName, ProjectFolder projectFolder)
        {
            ProjectName = projectName;
            ProjectFolder = projectFolder;
        }

        public ProjectName ProjectName { get; }
        public ProjectFolder ProjectFolder { get; }
    }
}