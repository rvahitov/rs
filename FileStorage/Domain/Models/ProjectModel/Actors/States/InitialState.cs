using System.Collections.Generic;
using Common.ExecutionResults;
using Common.Specifications;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal sealed class InitialState : IProjectState
    {
        public InitialState()
        {
            CommandSpecification = Specification.Create<IProjectCommand>( cmd => cmd is CreateProject, "Project does not exist" );
        }

        public ISpecification<IProjectCommand> CommandSpecification { get; }

        public Project? GetProject() => null;

        public IExecutionResult<IEnumerable<IProjectEvent>> RunCommand( IProjectCommand command )
        {
            var create = (CreateProject) command;
            return ExecutionResult.Success( new IProjectEvent[] { new ProjectCreated( create.ProjectName, create.ProjectFolder ) } );
        }

        public IProjectState ApplyEvent( IProjectEvent projectEvent )
        {
            if ( projectEvent is ProjectCreated created ) return ProjectState.Created( created.ProjectName, created.ProjectFolder );
            return this;
        }

    }
}