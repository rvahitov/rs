using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace Domain.SpecFlow.Steps
{
    [ Binding ]
    public class CreateProjectStepsDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CreateProjectStepsDefinitions( ScenarioContext scenarioContext )
        {
            _scenarioContext = scenarioContext;
        }

        [ Given( "необходимо создать проект (.*) с папкой (.*)" ) ]
        public void NeedToCreateProject( string projectNameValue, string projectDirectoryPath )
        {
            _scenarioContext.Pending();
        }

        [ Given( "в системе отсутствует проект с таким именем" ) ]
        public void AndSystemDoesNotHaveProjectWithName()
        {
            _scenarioContext.Pending();
        }

        [ When( "я отправляю в систему команду CreateProject" ) ]
        public void SendCreateProjectCommand()
        {
            _scenarioContext.Pending();
        }

        [ Then( "в системе появляется новый проект с именем (.*) и с папкой (.*)" ) ]
        public Task SystemShouldContainProject( string projectNameValue, string projectDirectoryPath )
        {
            _scenarioContext.Pending();
            return Task.CompletedTask;
        }
    }
}