namespace Domain.Models.ProjectModel.Actors.Events
{
    internal sealed class ProjectFileAdded : IProjectEvent
    {
        public ProjectFileAdded( int fileId, int fileSize )
        {
            FileId   = fileId;
            FileSize = fileSize;
        }

        public int FileId   { get; }
        public int FileSize { get; }
    }
}