using System;
using System.Collections.Generic;

namespace Domain.Models.ProjectModel
{
    public sealed class Project
    {
        public Project( ProjectName name, ProjectFolder folder, IReadOnlyCollection<int> files, long totalFileSize )
        {
            if ( totalFileSize < 0 ) throw new ArgumentOutOfRangeException( nameof( totalFileSize ) );
            Name          = name   ?? throw new ArgumentNullException( nameof( name ) );
            Folder        = folder ?? throw new ArgumentNullException( nameof( folder ) );
            Files         = files  ?? throw new ArgumentNullException( nameof( files ) );
            TotalFileSize = totalFileSize;
        }

        public ProjectName              Name          { get; }
        public ProjectFolder            Folder        { get; }
        public IReadOnlyCollection<int> Files         { get; }
        public long                     TotalFileSize { get; }
    }
}