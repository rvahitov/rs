using System;

namespace Domain.Models.ProjectModel
{
    public sealed class ProjectFolder
    {
        public ProjectFolder(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) throw new ArgumentException("Value cannot be null or whitespace.", nameof(path));
            Path = path;
        }

        public string Path { get; }

        private bool Equals(ProjectFolder other) => Path == other.Path;

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ProjectFolder other && Equals(other);

        public override int GetHashCode() => Path.GetHashCode();

        public static bool operator ==(ProjectFolder? left, ProjectFolder? right) => Equals(left, right);

        public static bool operator !=(ProjectFolder? left, ProjectFolder? right) => !Equals(left, right);
    }
}