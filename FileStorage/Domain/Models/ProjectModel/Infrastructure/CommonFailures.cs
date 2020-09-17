using Common.ExecutionResults;

namespace Domain.Models.ProjectModel.Infrastructure
{
    internal static class CommonFailures
    {
        public static IExecutionResult UnknownMessage => ExecutionResult.Failed( "Unknown message" );
    }
}