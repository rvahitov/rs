using System;

namespace Domain.Models.ProjectModel.Commands
{
    public sealed class AddProjectFile : IProjectCommand
    {
        public ProjectName          ProjectName { get; }
        public byte[] FileContent { get; }

        public AddProjectFile( ProjectName projectName, byte[] fileContent )
        {
            ProjectName = projectName ?? throw new ArgumentNullException( nameof( projectName ) );
            FileContent = fileContent;
        }
    }
}