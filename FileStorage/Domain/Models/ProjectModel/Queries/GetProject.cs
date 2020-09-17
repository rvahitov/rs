namespace Domain.Models.ProjectModel.Queries
{
    public sealed class GetProject : IHaveProjectName
    {
        public ProjectName ProjectName { get; }

        public GetProject(ProjectName projectName)
        {
            ProjectName = projectName;
        }
    }
}