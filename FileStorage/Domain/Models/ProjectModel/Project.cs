using System;

namespace Domain.Models.ProjectModel
{
    public sealed class Project
    {
        public Project(ProjectName name, ProjectFolder folder)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Folder = folder ?? throw new ArgumentNullException(nameof(folder));
        }

        public ProjectName Name { get; }
        public ProjectFolder Folder { get; }
    }
}