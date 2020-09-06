using System;
using Domain.Models.ProjectModel;
using Xunit;

namespace Domain.Tests
{
    /*
     У меня есть ProjectModule.
     ProjectModule имеет класс состояния проекта.
     Когда я проецирую комманду Create на состояние
     */
    public class ProjectModuleTests
    {
        [Fact]
        public void TestCreateProjectNameShouldTrhow()
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
    }
}