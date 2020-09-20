using System.Text;
using System.Threading.Tasks;
using Common.ExecutionResults;
using Domain.Application;
using Domain.Models.ProjectModel;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Domain.SpecFlow.Steps
{
    [Binding]
    public class RequestProjectFileStepsDefinitions
    {
        private readonly FileStorageApplication _application;
        private IExecutionResult<byte[]> _requestedFileContent;
        private ProjectName _projectName;

        public RequestProjectFileStepsDefinitions(FileStorageApplication application)
        {
            _application = application;
        }

        [Given("имеется проект с именем (.*) и с папкой (.*)")]
        public void GivenProject(string projectNameValue, string projectFolderPath)
        {
            _projectName = new ProjectName(projectNameValue);
            var projectFolder = new ProjectFolder(projectFolderPath);
            _application.CreateProject(_projectName, projectFolder);
        }

        [Given("и с содержимым (.*)")]
        public void GivenProjectFileContent(string fileContent)
        {
            var bytes = Encoding.UTF8.GetBytes(fileContent);
            _application.AddProjectFile(_projectName, bytes);
        }

        [When("я запрашиваю файл проекта с ИД (\\d+)")]
        public async Task WhenIRequestContentOfFile(int fileId)
        {
            _requestedFileContent = await _application.GetProjectFileContent(_projectName, fileId);
        }

        [Then("я должен получить Failure")]
        public void ThenIShouldGetFailure()
        {
            _requestedFileContent.IsSuccess.Should().BeFalse();
            _requestedFileContent.Errors.Should().ContainMatch("Project does not have file with Id *");
        }

        [Then("я должен получить Success с содержимым (.*)")]
        public void ThenIShouldGetSuccess(string fileContent)
        {
            var bytes = Encoding.UTF8.GetBytes(fileContent);
            _requestedFileContent.IsSuccess.Should().BeTrue();
            _requestedFileContent.SuccessValue.Should().ContainInOrder(bytes);
        }
    }
}