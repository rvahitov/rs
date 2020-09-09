using System;
using System.Collections.Generic;
using Common.ExecutionResults;
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
        }

        public Project GetProject() => new Project( _projectName, _projectFolder );

        public IExecutionResult<IEnumerable<IProjectEvent>> GetEventsForCommand( IProjectCommand command ) =>
            ExecutionResult.Failed<IEnumerable<IProjectEvent>>( "Not implemented" );

        public IProjectState Apply( IProjectEvent projectEvent ) => this;
    }
}