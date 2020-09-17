using Akka.Actor;

namespace Domain.Models.ProjectModel.Actors
{
    internal sealed class ProjectAggregateManager : UntypedActor
    {
        private IActorRef FindOrCreateChild( ProjectName projectName )
        {
            var child = Context.Child( projectName.Value );
            if ( ! child.IsNobody() ) return child;
            var props = Props.Create( () => new ProjectAggregate( projectName.Value ) );
            child = Context.ActorOf( props, projectName.Value );
            return child;
        }

        protected override void OnReceive( object message )
        {
            var msg = (IHaveProjectName) message;
            Forward( msg.ProjectName, msg );
        }

        private void Forward( ProjectName to, object message )
        {
            var child = FindOrCreateChild( to );
            child.Forward( message );
        }
    }
}