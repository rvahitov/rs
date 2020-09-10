using Akka.Actor;
using Autofac;
using Domain.Application;
using Domain.Application.Actors;

namespace Domain.Infrastructure
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                {
                    var props = Props.Create(() => new ApplicationActor());
                    var sys = c.Resolve<ActorSystem>();
                    return sys.ActorOf(props, "Application");
                })
                .Keyed<IActorRef>(typeof(ApplicationActor))
                .SingleInstance();

            builder.Register(c => new FileStorageApplication(c.ResolveKeyed<IActorRef>(typeof(ApplicationActor))));
        }
    }
}