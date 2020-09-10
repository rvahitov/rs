using Akka.Actor;
using Akka.TestKit.Xunit2;
using Common.ExecutionResults;
using Domain.Application.Actors;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;
using Xunit;

namespace Domain.SpecFlow.UnitTests
{
    public class ApplicationActorTests : TestKit
    {
        [Fact]
        public void ShouldDispatchProjectMessages()
        {
            var application = Sys.ActorOf(Props.Create<ApplicationActor>(), "Application");
            application.Tell(new CreateProject(new ProjectName("Prj1"), new ProjectFolder("2")));
            ExpectMsg<IExecutionResult>();
            application.Tell(new GetProject(new ProjectName("Prj1")));
            ExpectMsg<IExecutionResult<Project>>();
        }

        [Fact]
        public void ShouldNotHandleUnknownMessage()
        {
            var application = Sys.ActorOf(Props.Create<ApplicationActor>(), "Application");
            application.Tell( "Some string message", TestActor );
            var res = ExpectMsg<IExecutionResult>();
            Assert.False( res.IsSuccess );
        }
    }
}