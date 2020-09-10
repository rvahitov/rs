using System.Threading.Tasks;
using Domain.Application;
using Domain.Models.ProjectModel;
using TechTalk.SpecFlow;
using Xunit;

namespace Domain.SpecFlow.Steps
{
    [Binding]
    public class CreateProjectStepsDefinitions
    {
        private readonly FileStorageApplication _application;
        private ProjectName _projectName = null;
        private ProjectFolder _projectFolder = null;

        public CreateProjectStepsDefinitions(FileStorageApplication application)
        {
            _application = application;
        }

        [Given("необходимо создать проект (.*) с папкой (.*)")]
        public void NeedToCreateProject(string projectNameValue, string projectFolderPath)
        {
            _projectName = new ProjectName(projectNameValue);
            _projectFolder = new ProjectFolder(projectFolderPath);
        }

        [Given("в системе отсутствует проект с таким именем")]
        public void AndSystemDoesNotHaveProjectWithName()
        {
            Assert.False(_application.ProjectExists(_projectName));
        }

        [When("я отправляю в систему команду CreateProject")]
        public void SendCreateProjectCommand()
        {
            _application.CreateProject(_projectName, _projectFolder);
        }

        [Then("в системе появляется новый проект с именем (.*) и с папкой (.*)")]
        public async Task SystemShouldContainProject(string projectNameValue, string projectDirectoryPath)
        {
            var projectName = new ProjectName(projectNameValue);
            var getResult = await _application.GetProject(projectName);
            Assert.True(getResult.IsSuccess);
            var project = getResult.SuccessValue;
            Assert.NotNull(project);
            Assert.Equal(projectName, project.Name);
            Assert.Equal(new ProjectFolder(projectDirectoryPath), project.Folder);
        }
    }
}