using System.Collections.Generic;
using Common.ExecutionResults;
using Common.Specifications;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal interface IProjectState
    {
        Project? GetProject();

        ISpecification<IProjectCommand> CommandSpecification { get; }

        IExecutionResult<IEnumerable<IProjectEvent>> RunCommand( IProjectCommand command );

        IProjectState ApplyEvent( IProjectEvent projectEvent );
    }
}