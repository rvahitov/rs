using System;
using Domain.Models.ProjectModel;
using Xbehave;
using Xunit;
// ReSharper disable AssignNullToNotNullAttribute

namespace Domain.SpecFlow.UnitTests
{
    public sealed class ProjectFolderTests
    {
        private const string PathTestValue = "C:\\temp";

        [Scenario]
        public void Create()
        {
            string        path   = null;
            ProjectFolder folder = null;
            $"Given: I have path to folder {PathTestValue}"
                .x( () => path = PathTestValue );
            "When I create ProjectFolder object"
                .x( () => folder = new ProjectFolder( path ) );
            $"Then object's property Value should be equal to {PathTestValue}"
                .x( () => Assert.Equal( PathTestValue, folder.Path ) );
        }

        [Scenario]
        [Example(null)]
        [Example("")]
        [Example("   ")]
        public void CreateShouldFail(string path)
        {
            Exception createException = null;
            $"Given: I have empty string \"{path ?? "null"}\""
                .x( () => Assert.True( string.IsNullOrWhiteSpace( path ) ) );
            "When I create ProjectFolder object with this string"
                .x( () =>createException = Assert.Throws<ArgumentException>( () => new ProjectFolder( path ) ) );
            "Then I should get ArgumentException exception"
                .x( () => Assert.IsType<ArgumentException>( createException ) );
        }

        [Scenario]
        [Example(@"C:\temp\Project1")]
        [Example(@"C:\temp\Project2")]
        [Example(@"D:\temp\Project3")]
        public void Equality( string path )
        {
            ProjectFolder p1 = null;
            ProjectFolder p2 = null;
            $"Given: I have not empty path {path}".x( () => Assert.True( ! string.IsNullOrWhiteSpace( path ) ) );
            "When I create two ProjectFolder objects with this path"
                .x( () =>
                {
                    p1 = new ProjectFolder( path );
                    p2 = new ProjectFolder( path );
                } );
            "Then this two objects should be equal and have the same HashCode"
                .x( () =>
                {
                    Assert.Equal( p1, p2 );
                    Assert.True( p1 == p2 );
                    Assert.Equal( p1.GetHashCode(), p2.GetHashCode() );
                } );

            string anotherPath = null;
            $"Given: I have another path {path + "_100000"}"
                .x( () => anotherPath = path + "_100000" );
            "When I create two ProjectFolder objects with this two paths"
                .x( () =>
                {
                    p1 = new ProjectFolder( path );
                    p2 = new ProjectFolder( anotherPath );
                } );
            "Then this two object should be not equal and should not have the same HashCode"
                .x( () =>
                {
                    Assert.NotEqual( p1, p2 );
                    Assert.True( p1 != p2 );
                    Assert.NotEqual( p1.GetHashCode(), p2.GetHashCode() );
                } );
        }
    }
}