using System;

namespace Domain.Models.ProjectModel.Commands
{
    public sealed class CreateProject: IProjectCommand
    {
        public CreateProject(ProjectName projectName, ProjectFolder projectFolder)
        {
            ProjectName = projectName ?? throw new ArgumentNullException(nameof(projectName));
            ProjectFolder = projectFolder ?? throw new ArgumentNullException(nameof(projectFolder));
        }

        public ProjectName ProjectName { get; }
        public ProjectFolder ProjectFolder { get; }
    }
}