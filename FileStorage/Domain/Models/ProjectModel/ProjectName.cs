namespace Domain.Models.ProjectModel
{
    public sealed class ProjectName
    {
        public ProjectName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public override bool Equals(object? obj) => ReferenceEquals(this, obj) || obj is ProjectName other && Equals(Value, other.Value);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(ProjectName? left, ProjectName? right) => Equals(left, right);

        public static bool operator !=(ProjectName? left, ProjectName? right) => !Equals(left, right);
    }
}