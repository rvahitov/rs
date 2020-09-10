using Akka.Actor;
using Common.ExecutionResults;
using Domain.Models.ProjectModel.Actors;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Application.Actors
{
    internal class ApplicationActor : UntypedActor
    {
        private IActorRef _projectAggregateManager = Nobody.Instance;

        protected override void PreStart()
        {
            _projectAggregateManager =
                Context.ActorOf( Props.Create( () => new ProjectAggregateManager() ), "ProjectManager" );
        }

        protected override void OnReceive( object message )
        {
            switch ( message )
            {
                case IProjectCommand cmd :
                    _projectAggregateManager.Forward( cmd );
                    break;
                case GetProject query :
                    _projectAggregateManager.Forward( query );
                    break;
                default :
                    Unhandled( message );
                    Sender.Tell( ExecutionResult.Failed( "Unknown message" ) );
                    break;
            }
        }
    }
}