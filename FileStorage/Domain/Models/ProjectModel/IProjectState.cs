using Common.ExecutionResults;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Events;

namespace Domain.Models.ProjectModel
{
    internal interface IProjectState
    {
        IExecutionResult<IProjectEvent[]> GenerateEvents(IProjectCommand cmd);
    }

    internal static class ProjectState
    {
        public static IProjectState Initial() => new InitialState();
    }

    internal sealed class InitialState : IProjectState
    {
        public IExecutionResult<IProjectEvent[]> GenerateEvents(IProjectCommand cmd)
        {
            if (cmd is CreateProject create)
            {
                return ExecutionResult.Success(new IProjectEvent[] {new ProjectCreated(create.ProjectName, create.ProjectFolder),});
            }

            return ExecutionResult.Failed<IProjectEvent[]>("Project is not created");
        }
    }
}