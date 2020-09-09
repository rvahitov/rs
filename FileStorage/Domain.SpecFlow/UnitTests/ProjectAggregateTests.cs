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
    public sealed class ProjectAggregateTests : TestKit
    {
        [ Scenario ]
        public void Create()
        {
            IActorRef     projectAggregate = Nobody.Instance;
            ProjectName   projectName      = null;
            ProjectFolder projectFolder    = null;

            "Given: I have ProjectAggregate actor with initial state"
                .x( () =>
                {
                    var props = Props.Create( () => new ProjectAggregate( "Project1" ) );
                    projectAggregate = Sys.ActorOf( props, "Project1" );
                } );

            "And I have project name and I have project folder"
                .x( () =>
                {
                    projectName   = new ProjectName( "TestProject" );
                    projectFolder = new ProjectFolder( @"c:\temp\test_project" );
                } );

            "When I send Create command to aggregate"
                .x( () =>
                {
                    var command = new CreateProject( projectName, projectFolder );
                    projectAggregate.Tell( command, TestActor );
                } );

            "Then I should receive success result"
                .x( () =>
                {
                    var result = ExpectMsg<IExecutionResult>();
                    Assert.True( result.IsSuccess );
                } );

            "After that when I ask for project"
                .x( () =>
                {
                    var query = new GetProject(projectName);
                    projectAggregate.Tell( query, TestActor );
                } );

            "Then I should get project with the same name and folder"
                .x( () =>
                {
                    var result = ExpectMsg<IExecutionResult<Project>>();
                    Assert.True( result.IsSuccess );
                    Assert.Equal( projectName, result.SuccessValue.Name );
                    Assert.Equal( projectFolder, result.SuccessValue.Folder );
                } );

            "Now I have ProjectAggregateActor with created state".x( () => { } );

            "And I have same project name and I have different project folder"
                .x( () =>
                {
                    projectName   = new ProjectName( "TestProject" );
                    projectFolder = new ProjectFolder( @"c:\temp\TestProject" );
                } );

            "When I again send Create command to aggregate"
                .x( () =>
                {
                    var command = new CreateProject( projectName, projectFolder );
                    projectAggregate.Tell( command, TestActor );
                } );

            "Then I should get failure result"
                .x( () =>
                {
                    var result = ExpectMsg<IExecutionResult>();
                    Assert.False( result.IsSuccess );
                } );

            "After that when I ask for project"
                .x( () =>
                {
                    var query = new GetProject(projectName);
                    projectAggregate.Tell( query, TestActor );
                } );

            "Then I should get project with old name and old folder"
                .x( () =>
                {
                    var result = ExpectMsg<IExecutionResult<Project>>();
                    Assert.True( result.IsSuccess );
                    Assert.Equal( "TestProject", result.SuccessValue.Name.Value );
                    Assert.Equal( @"c:\temp\test_project", result.SuccessValue.Folder.Path );
                } );
        }
    }
}