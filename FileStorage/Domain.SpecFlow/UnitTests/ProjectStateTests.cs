using System.Collections.Generic;
using Common.ExecutionResults;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Actors.States;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;
using Xbehave;
using Xunit;

namespace Domain.SpecFlow.UnitTests
{
    public sealed class ProjectStateTests
    {
        [Scenario]
        public void GetProject()
        {
            IProjectState state   = null;
            Project       project = null;
            "I have initial ProjectState".x( () => state = ProjectState.Initial() );
            "When I ask for Project".x( () => project        = state.GetProject() );
            "Then I should get null".x( () => Assert.Null( project ) );

            "I have created ProjectState"
                .x( () => state = new CreatedState( new ProjectName( "Pr1" ), new ProjectFolder( "Dir2" ) ) );
            "When I ask for Project".x( () => project        = state.GetProject() );
            "Then I should get project with name \"Pr1\" and folder \"Dir2\""
                .x( () =>
                {
                    Assert.NotNull( project );
                    Assert.Equal( "Pr1", project.Name.Value );
                    Assert.Equal( "Dir2", project.Folder.Path );
                } );
        }

        [Scenario]
        public void GetEvents()
        {
            IProjectState                                state           = null;
            IProjectCommand                              command         = null;
            IExecutionResult<IEnumerable<IProjectEvent>> getEventsResult = null;

            "I have initial ProjectState".x( () => state                 = ProjectState.Initial() );
            "And I have CreateCommand".x( () => command                  = new CreateProject( new ProjectName( "ProjectName" ), new ProjectFolder( "ProjectFolder" ) ) );
            "When I ask for events for command".x( () => getEventsResult = state.GetEventsForCommand( command ) );
            "Then I should success result with one event: ProjectCreated"
                .x( () =>
                {
                    Assert.NotNull( getEventsResult );
                    Assert.True( getEventsResult.IsSuccess );
                    Assert.Collection( getEventsResult.SuccessValue, e =>
                    {
                        var projectCreated = Assert.IsType<ProjectCreated>( e );
                        Assert.Equal( "ProjectName", projectCreated.ProjectName.Value );
                        Assert.Equal( "ProjectFolder", projectCreated.ProjectFolder.Path );
                    } );
                } );
        }

        [Scenario]
        public void ApplyEvent()
        {
            IProjectState state = null;

            IProjectEvent projectEvent = null;

            IProjectState newState = null;

            "I have initial project state".x( () => state           = ProjectState.Initial() );
            "And I have event ProjectCreated".x( () => projectEvent = new ProjectCreated( new ProjectName( "ProjectName" ), new ProjectFolder( "ProjectFolder" ) ) );
            "When I ask to apply event".x( () => newState           = state.Apply( projectEvent ) );
            "Then I should get CreatedState"
                .x( () =>
                {
                    Assert.IsType<CreatedState>( newState );
                } );

            "I have initial project state".x( () => state = ProjectState.Initial() );
            "And I have fake event".x( () => projectEvent = new FakeEvent() );
            "When I ask to apply event".x( () => newState           = state.Apply( projectEvent ) );
            "Then I should get old state".x( () => Assert.Equal( state, newState ) );

            ProjectName projectName = new ProjectName("ProjectName");
            ProjectFolder projectFolder = new ProjectFolder("ProjectFolder");

            "I have created project state".x( () => state    = ProjectState.Created( projectName, projectFolder ) );
            "And I have Created event".x( () => projectEvent = new ProjectCreated( new ProjectName( "Fake" ), new ProjectFolder( "FolderFake" ) ) );
            "When I ask to apply event".x( () => newState           = state.Apply( projectEvent ) );
            "Then I should get old state".x( () => Assert.Equal( state, newState ) );
        }

        private sealed class FakeEvent : IProjectEvent
        {
        }
    }
}