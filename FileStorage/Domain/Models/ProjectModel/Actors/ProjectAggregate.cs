using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Common.ExecutionResults;
using Domain.Models.ProjectModel.Actors.States;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Models.ProjectModel.Actors
{
    public sealed class ProjectAggregate : ReceivePersistentActor
    {
        private IProjectState _state;

        public ProjectAggregate( string persistenceId )
        {
            PersistenceId = persistenceId;
            _state        = ProjectState.Initial();

            Command<CreateProject>( OnCreateCommand );

            Command<GetProject>( _ => OnGetQuery() );
        }

        public override string PersistenceId { get; }

        private void OnCreateCommand( CreateProject cmd )
        {
            var events = _state.GetEventsForCommand( cmd );
            if ( events.IsSuccess )
            {
                PersistAll( events.SuccessValue, e =>
                {
                    _state = _state.Apply( e );
                } );
            }

            Sender.Tell( events.IsSuccess ? ExecutionResult.Success() : ExecutionResult.Failed( events.Errors.ToArray() ) );
        }

        private void OnGetQuery()
        {
            Sender.Tell( ExecutionResult.Success( _state.GetProject() ) );
        }
    }
}