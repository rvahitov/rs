namespace Domain.Models.ProjectModel.Queries
{
    public sealed class GetProject : IProjectQuery
    {
        public ProjectName ProjectName { get; }

        public GetProject(ProjectName projectName)
        {
            ProjectName = projectName;
        }
    }
}