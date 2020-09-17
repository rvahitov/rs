using System;

namespace Domain.Models.ProjectModel.Commands
{
    public sealed class AddProjectFile : IProjectCommand
    {
        public ProjectName          ProjectName { get; }
        public ReadOnlyMemory<byte> FileContent { get; }

        public AddProjectFile( ProjectName projectName, in ReadOnlyMemory<byte> fileContent )
        {
            ProjectName = projectName ?? throw new ArgumentNullException( nameof( projectName ) );
            FileContent = fileContent;
        }
    }
}