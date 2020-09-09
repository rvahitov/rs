using Akka.Actor;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Models.ProjectModel.Actors
{
    internal sealed class ProjectAggregateManager : ReceiveActor
    {
        public ProjectAggregateManager()
        {
            Receive<IProjectCommand>( cmd =>
            {
                var child = FindOrCreateChild( cmd.ProjectName );
                child.Forward( cmd );
            } );

            Receive<GetProject>( query =>
            {
                var child = FindOrCreateChild( query.ProjectName );
                child.Forward( query );
            } );
        }

        private IActorRef FindOrCreateChild( ProjectName projectName )
        {
            var child = Context.Child( projectName.Value );
            if ( ! child.IsNobody() ) return child;
            var props = Props.Create( () => new ProjectAggregate( projectName.Value ) );
            child = Context.ActorOf( props, projectName.Value );
            return child;
        }
    }
}