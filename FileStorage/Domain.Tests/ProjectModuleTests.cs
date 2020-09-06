using System;
using System.Linq;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;
using Xunit;

namespace Domain.Tests
{
    public class ProjectModuleTests
    {
        [Fact]
        public void TestCreateProjectNameShouldThrow()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new ProjectName(null));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new ProjectName(""));
            Assert.Throws<ArgumentException>(() => new ProjectName("   "));
        }

        [Fact]
        public void TestProjectNameEquality()
        {
            var projectName = new ProjectName("TestProject");
            Assert.True(projectName == new ProjectName("TestProject"));
            Assert.Equal("TestProject", projectName.Value);
            Assert.Equal("TestProject".GetHashCode(), projectName.GetHashCode());
            Assert.True(projectName != new ProjectName("Foo"));
        }

        [Fact]
        public void TestCreateProjectFolderShouldThrow()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new ProjectFolder(null));
            // ReSharper restore AssignNullToNotNullAttribute
            Assert.Throws<ArgumentException>(() => new ProjectFolder(""));
            Assert.Throws<ArgumentException>(() => new ProjectFolder("  "));
        }

        [Fact]
        public void TestProjectFolderEquality()
        {
            const string path = @"C:\Develop";
            var projectFolder = new ProjectFolder(path);
            Assert.True(projectFolder == new ProjectFolder(path));
            Assert.False(projectFolder != new ProjectFolder(path));
            Assert.Equal(path, projectFolder.Path);
            Assert.Equal(projectFolder.GetHashCode(), new ProjectFolder(path).GetHashCode());
        }

        [Fact]
        public void CreateProjectCtor_ShouldFail()
        {
            // ReSharper disable AssignNullToNotNullAttribute
            Assert.Throws<ArgumentNullException>(() => new CreateProject(null, new ProjectFolder(@"C:\Test")));
            Assert.Throws<ArgumentNullException>(() => new CreateProject(new ProjectName("Test"), null));
            // ReSharper restore AssignNullToNotNullAttribute
        }

        [Fact]
        public void WhenInitialState_OnlyCreateCommandAcceptable()
        {
            var projectName = new ProjectName("Project1");
            var projectFolder = new ProjectFolder("C:\\Project1");
            var state = ProjectState.Initial();
            var create = new CreateProject(projectName, projectFolder);
            Assert.Equal(projectName, create.ProjectName);
            Assert.Equal(projectFolder, create.ProjectFolder);
            var result = state.GenerateEvents(create);
            Assert.True(result.IsSuccess);
            var createdEvents = result.SuccessValue.OfType<ProjectCreated>().ToArray();
            Assert.Single(createdEvents);
            var created = createdEvents[0];
            Assert.Equal(projectName, created.ProjectName);
            Assert.Equal(projectFolder, created.ProjectFolder);

            result = state.GenerateEvents(new FakeCommand());
            Assert.False(result.IsSuccess);
        }
        

        private sealed class FakeCommand : IProjectCommand
        {
        }
    }
}