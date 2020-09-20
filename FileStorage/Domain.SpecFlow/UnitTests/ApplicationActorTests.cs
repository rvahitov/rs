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
        [ Fact ]
        public void ShouldDispatchProjectMessages()
        {
            var application   = Sys.ActorOf( Props.Create<ApplicationActor>(), "Application" );
            var projectName   = new ProjectName( "Prj1" );
            var projectFolder = new ProjectFolder( "C:\\temp\\2" );
            application.Tell( new CreateProject( projectName, projectFolder ), TestActor );
            ExpectMsg<IExecutionResult>();
            application.Tell( new GetProject( projectName ), TestActor );
            ExpectMsg<IExecutionResult<Project>>();

            application.Tell( new AddProjectFile( projectName,  new byte[] { 120, 121, 122, 123 } ) , TestActor );
            ExpectMsg<IExecutionResult>(r => r.IsSuccess);
        }

        [ Fact ]
        public void ShouldNotHandleUnknownMessage()
        {
            var application = Sys.ActorOf( Props.Create<ApplicationActor>(), "Application" );
            application.Tell( "Some string message", TestActor );
            var res = ExpectMsg<IExecutionResult>();
            Assert.False( res.IsSuccess );
        }
    }
}