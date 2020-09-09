namespace Domain.Models.ProjectModel.Queries
{
    public sealed class GetProject
    {
        public ProjectName ProjectName { get; }

        public GetProject(ProjectName projectName)
        {
            ProjectName = projectName;
        }
    }
}