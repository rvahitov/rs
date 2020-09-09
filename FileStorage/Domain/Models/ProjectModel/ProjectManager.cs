using System.Threading.Tasks;
using Domain.Models.ProjectModel.Commands;

namespace Domain.Models.ProjectModel
{
    internal class ProjectManager
    {
        public void SendCommand(IProjectCommand command)
        {
        }

        public Task<Project?> GetProject(ProjectName name)
        {
            return Task.FromResult<Project?>(null);
        }
    }
}