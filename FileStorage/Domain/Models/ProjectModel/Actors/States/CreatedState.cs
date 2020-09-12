using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using Common.ExecutionResults;
using Common.Specifications;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal sealed class CreatedState : IProjectState
    {
        private readonly ProjectName        _projectName;
        private readonly ProjectFolder      _projectFolder;
        private readonly ImmutableList<int> _files;
        private readonly long               _totalFileSize;

        public CreatedState( ProjectName projectName, ProjectFolder projectFolder, ImmutableList<int> files, long totalFileSize )
        {
            _projectName   = projectName   ?? throw new ArgumentNullException( nameof( projectName ) );
            _projectFolder = projectFolder ?? throw new ArgumentNullException( nameof( projectFolder ) );
            _files         = files         ?? throw new ArgumentNullException( nameof( files ) );
            _totalFileSize = totalFileSize;

            CommandSpecification = Specification.Create<IProjectCommand>( cmd => ! (cmd is CreateProject), $"Project {projectName.Value} already exists" );
        }

        public ISpecification<IProjectCommand> CommandSpecification { get; }

        public Project GetProject() => new Project( _projectName, _projectFolder, _files, _totalFileSize );

        public IExecutionResult<IEnumerable<IProjectEvent>> RunCommand( IProjectCommand command )
        {
            return RunAddFile( (AddProjectFile) command );
        }

        private IExecutionResult<IEnumerable<IProjectEvent>> RunAddFile( AddProjectFile command )
        {
            var fileId = _files.Count;

            var directoryPah = Path.GetFullPath( _projectFolder.Path );
            if ( ! Directory.Exists( directoryPah ) ) Directory.CreateDirectory( directoryPah );
            var filePath = Path.Combine( directoryPah, $"{_projectName.Value}_{fileId}" );
            File.WriteAllBytes( filePath, command.FileContent.ToArray() );
            return ExecutionResult.Success( new IProjectEvent[] { new ProjectFileAdded( fileId, command.FileContent.Length ) } );
        }

        public IProjectState ApplyEvent( IProjectEvent projectEvent ) => this;
    }
}