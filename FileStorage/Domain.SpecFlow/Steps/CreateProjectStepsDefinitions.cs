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
        private          ProjectName            _projectName;
        private          ProjectFolder          _projectFolder;

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
        public async Task AndSystemDoesNotHaveProjectWithName()
        {
            var result = await _application.ProjectExists( _projectName );
            Assert.False(result);
        }

        [Given("в системе уже есть проект с именем (.*) и с папкой (.*)")]
        public void AndSystemAlreadyHaveProject(string projectNameValue, string projectFolderPath)
        {
            _application.CreateProject( new ProjectName( projectNameValue ), new ProjectFolder( projectFolderPath ) );
        }

        [When("я отправляю в систему команду CreateProject")]
        public void SendCreateProjectCommand()
        {
            _application.CreateProject(_projectName, _projectFolder);
        }

        [Then("в системе появляется новый проект с данным именем и папкой")]
        public async Task SystemShouldContainProject()
        {
            var getResult = await _application.GetProject(_projectName);
            Assert.True(getResult.IsSuccess);
            var project = getResult.SuccessValue;
            Assert.NotNull(project);
            Assert.Equal(_projectName, project.Name);
            Assert.Equal(_projectFolder, project.Folder);
        }

        [Then("в системе не должен появлятся новый проект с данным именем и папкой")]
        public async Task SystemShouldNotContainProjectDuplicate()
        {
            var getResult = await _application.GetProject(_projectName);
            Assert.True(getResult.IsSuccess);
            var project = getResult.SuccessValue;
            Assert.NotNull(project);
            Assert.Equal(_projectName, project.Name);
            Assert.NotEqual(_projectFolder, project.Folder);
        }
    }
}