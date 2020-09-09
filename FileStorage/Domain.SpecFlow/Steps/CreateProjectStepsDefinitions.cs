using System.Threading.Tasks;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Commands;
using TechTalk.SpecFlow;
using Xunit;

namespace Domain.SpecFlow.Steps
{
    [Binding]
    public class CreateProjectStepsDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        private CreateProject _command;
        private ProjectManager _manager;

        public CreateProjectStepsDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("необходимо создать проект (.*) с папкой (.*)")]
        public void NeedToCreateProject(string projectNameValue, string projectDirectoryPath)
        {
            _command = new CreateProject(new ProjectName(projectNameValue), new ProjectFolder(projectDirectoryPath));
        }

        [Given("в системе отсутствует проект с таким именем")]
        public void AndSystemDoesNotHaveProjectWithName()
        {
            _manager = new ProjectManager();
        }

        [When("я отправляю в систему команду CreateProject")]
        public void SendCreateProjectCommand()
        {
            _manager.SendCommand(_command);
        }

        [Then("в системе появляется новый проект с именем (.*) и с папкой (.*)")]
        public async Task SystemShouldContainProject(string projectNameValue, string projectDirectoryPath)
        {
            var project = await _manager.GetProject(_command.ProjectName);
            Assert.NotNull(project);
            Assert.Equal(_command.ProjectName, project.Name);
            Assert.Equal(_command.ProjectFolder, project.Folder);
            _scenarioContext.Pending();
        }
    }
}