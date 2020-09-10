using System;
using System.Linq;
using Akka.Actor;
using Autofac;
using Domain.Infrastructure;
using SpecFlow.Autofac;
using TechTalk.SpecFlow;

namespace Domain.SpecFlow.Support
{
    internal static class IoCSupport
    {
        [ScenarioDependencies]
        public static ContainerBuilder CreateContainerBuilder()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule(new DomainModule());
            containerBuilder.RegisterTypes(typeof(IoCSupport).Assembly.GetTypes().Where(t => Attribute.IsDefined(t, typeof(BindingAttribute))).ToArray()).SingleInstance();
            containerBuilder.Register(_ => ActorSystem.Create("DomainTest"));
            return containerBuilder;
        }
    }
}