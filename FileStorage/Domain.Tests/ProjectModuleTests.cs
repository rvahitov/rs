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
        public void TestProjectNameEquality()
        {
            var projectName = new ProjectName("TestProject");
            Assert.True(projectName == new ProjectName("TestProject"));
            Assert.Equal("TestProject", projectName.Value);
            Assert.Equal("TestProject".GetHashCode(), projectName.GetHashCode());
            Assert.True(projectName != new ProjectName("Foo"));
        }
    }
}