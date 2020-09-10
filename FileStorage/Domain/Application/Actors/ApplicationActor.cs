using Akka.Actor;
using Domain.Models.ProjectModel.Actors;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Application.Actors
{
    internal class ApplicationActor : UntypedActor
    {
        private readonly IActorRef _projectAggregateManager;

        public ApplicationActor()
        {
            _projectAggregateManager =
                Context.ActorOf(Props.Create(() => new ProjectAggregateManager()), "ProjectManager");
        }

        protected override void OnReceive(object message)
        {
            switch (message)
            {
                case IProjectCommand cmd:
                    _projectAggregateManager.Forward(cmd);
                    break;
                case GetProject query:
                    _projectAggregateManager.Forward(query);
                    break;
                default:
                    Unhandled(message);
                    break;
            }
        }
    }
}