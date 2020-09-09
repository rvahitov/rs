using System.Collections.Generic;
using Common.ExecutionResults;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel.Actors.States
{
    internal interface IProjectState
    {
        Project? GetProject();

        IExecutionResult<IEnumerable<IProjectEvent>> GetEventsForCommand( IProjectCommand command );

        IProjectState Apply( IProjectEvent projectEvent );
    }
}