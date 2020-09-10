using System;
using System.Collections.Generic;
using System.IO;
using Common.ExecutionResults;
using Common.Specifications;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal sealed class CreatedState : IProjectState
    {
        private readonly ProjectName   _projectName;
        private readonly ProjectFolder _projectFolder;

        public CreatedState( ProjectName projectName, ProjectFolder projectFolder )
        {
            _projectName   = projectName   ?? throw new ArgumentNullException( nameof( projectName ) );
            _projectFolder = projectFolder ?? throw new ArgumentNullException( nameof( projectFolder ) );

            CommandSpecification = Specification.Create<IProjectCommand>( cmd => ! (cmd is CreateProject), $"Project {projectName.Value} already exists" );
        }

        public ISpecification<IProjectCommand> CommandSpecification { get; }

        public Project GetProject() => new Project( _projectName, _projectFolder );

        public IExecutionResult<IEnumerable<IProjectEvent>> RunCommand( IProjectCommand command )
        {
            return RunAddFile( (AddProjectFile) command );
        }

        private IExecutionResult<IEnumerable<IProjectEvent>> RunAddFile( AddProjectFile command )
        {
            var directoryPah = Path.GetFullPath( _projectFolder.Path );
            if ( ! Directory.Exists( directoryPah ) ) Directory.CreateDirectory( directoryPah );
            var filePath = Path.Combine( directoryPah, $"{_projectName.Value}_{1}" );
            File.WriteAllBytes( filePath, command.FileContent.ToArray() );
            return ExecutionResult.Success( new IProjectEvent[] { new ProjectFileAdded() } );
        }

        public IProjectState ApplyEvent( IProjectEvent projectEvent ) => this;
    }
}