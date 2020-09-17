using Akka.Actor;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Actors;
using Domain.Models.ProjectModel.Infrastructure;

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
            if ( message is IHaveProjectName )
            {
                _projectAggregateManager.Forward( message );
            }
            else
            {
                Unhandled( message );
                Sender.Tell( CommonFailures.UnknownMessage );
            }
        }
    }
}