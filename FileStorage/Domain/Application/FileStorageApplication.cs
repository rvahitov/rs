using System;
using System.Threading.Tasks;
using Akka.Actor;
using Common.ExecutionResults;
using Domain.Models.ProjectModel;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Application
{
    public class FileStorageApplication
    {
        private readonly IActorRef _applicationActor;

        public FileStorageApplication(IActorRef applicationActor)
        {
            _applicationActor = applicationActor;
        }

        public bool ProjectExists(ProjectName projectName)
        {
            return false;
        }

        public void CreateProject(ProjectName projectName, ProjectFolder projectFolder)
        {
        }

        public async Task<IExecutionResult<Project?>> GetProject(ProjectName projectName)
        {
            var query = new GetProject(projectName);
            var result = await _applicationActor.Ask<IExecutionResult<Project?>>(query).ConfigureAwait(false);
            return result;
        }
    }
}