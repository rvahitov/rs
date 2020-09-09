using Akka.Actor;
using Akka.TestKit.Xunit2;
using Common.ExecutionResults;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Actors;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Queries;
using Xbehave;
using Xunit;

namespace Domain.SpecFlow.UnitTests
{
    public sealed class ProjectAggregateManagerTests : TestKit
    {
        [ Scenario ]
        public void Create()
        {
            IActorRef projectAggregateManager = Nobody.Instance;

            "I have ProjectAggregateManager"
                .x( () =>
                {
                    var props = Props.Create( () => new ProjectAggregateManager() );
                    projectAggregateManager = Sys.ActorOf( props, "ProjectManager" );
                } );
            "When I send project command"
                .x( () => { projectAggregateManager.Tell( new CreateProject( new ProjectName( "ProjectId" ), new ProjectFolder( "ProjectFolder" ) ), TestActor ); } );
            "Then child project aggregate should be created"
                .x( () => { ExpectMsg<IExecutionResult>( ( er, sender ) => Assert.Contains( "ProjectId", sender.Path.ToString() ) ); } );

            "When I ask for project"
                .x( () => projectAggregateManager.Tell( new GetProject( new ProjectName( "ProjectId" ) ) ) );

            "Then I should get result from the same project aggregate"
                .x( () => { ExpectMsg<IExecutionResult<Project>>( ( r, sender ) => Assert.Contains( "ProjectId", sender.Path.ToString() ) ); } );
        }
    }
}