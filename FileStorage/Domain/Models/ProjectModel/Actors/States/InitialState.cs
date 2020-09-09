using System.Collections.Generic;
using System.Linq;
using Common.ExecutionResults;
using Common.Specifications;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal sealed class InitialState : IProjectState
    {
        private readonly ISpecification<IProjectCommand> _commandSpecification;

        public InitialState()
        {
            _commandSpecification = Specification.Create<IProjectCommand>( cmd => cmd is CreateProject, "Project does not exist" );
        }

        public Project? GetProject() => null;

        public IExecutionResult<IEnumerable<IProjectEvent>> GetEventsForCommand( IProjectCommand command )
        {
            var specificationResult = _commandSpecification.Check( command ).Map( () => (CreateProject) command )
                                                           .Map( cmd =>
                                                           {
                                                               var events = new IProjectEvent[] { new ProjectCreated( cmd.ProjectName, cmd.ProjectFolder ) };
                                                               return events.AsEnumerable();
                                                           } );
            return specificationResult;
        }

        public IProjectState Apply( IProjectEvent projectEvent )
        {
            if ( projectEvent is ProjectCreated created ) return ProjectState.Created( created.ProjectName, created.ProjectFolder );
            return this;
        }
    }
}