using System;

namespace Domain.Models.ProjectModel.Events
{
    public sealed class ProjectFileAdded : IProjectEvent
    {
        public ProjectFileAdded( int fileId, int fileSize )
        {
            if ( fileId   <= 0 ) throw new ArgumentOutOfRangeException( nameof( fileId ) );
            if ( fileSize <= 0 ) throw new ArgumentOutOfRangeException( nameof( fileSize ) );
            FileId   = fileId;
            FileSize = fileSize;
        }

        public int FileId   { get; }
        public int FileSize { get; }
    }
}