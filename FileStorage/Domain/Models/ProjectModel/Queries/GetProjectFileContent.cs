namespace Domain.Models.ProjectModel.Queries
{
    public class GetProjectFileContent : IProjectQuery
    {
        public GetProjectFileContent(ProjectName projectName, int fileId)
        {
            ProjectName = projectName;
            FileId = fileId;
        }

        public int FileId { get; }

        public ProjectName ProjectName { get; }
    }
}