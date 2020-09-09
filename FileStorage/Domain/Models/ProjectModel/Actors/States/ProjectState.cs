namespace Domain.Models.ProjectModel.Actors.States
{
    internal static class ProjectState
    {
        public static IProjectState Initial() => new InitialState();

        public static IProjectState Created(ProjectName projectName, ProjectFolder projectFolder) => new CreatedState( projectName, projectFolder );
    }
}