using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Common.ExecutionResults;
using Domain.Models.ProjectModel.Actors.States;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Infrastructure;
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

            CommandAny( cmd =>
            {
                switch ( cmd )
                {
                    case IProjectCommand command :
                        OnCommand( command );
                        break;
                    case GetProject _ :
                        Sender.Tell( ExecutionResult.Success( _state.GetProject() ) );
                        break;
                    default :
                        Sender.Tell(CommonFailures.UnknownMessage);
                        Unhandled( cmd );
                        break;
                }
            } );
        }

        public override string PersistenceId { get; }

        private void OnCommand( IProjectCommand cmd )
        {
            var canRun = _state.CommandSpecification.Check( cmd );

            if ( canRun.IsSuccess == false )
            {
                Sender.Tell( canRun );
                return;
            }

            var events = _state.RunCommand( cmd );
            Sender.Tell( events.IsSuccess ? ExecutionResult.Success() : ExecutionResult.Failed( events.Errors.ToArray() ) );

            if ( events.IsSuccess ) PersistAll( events.SuccessValue, e => { _state = _state.ApplyEvent( e ); } );

        }
    }
}