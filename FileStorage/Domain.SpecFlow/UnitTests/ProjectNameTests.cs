using System;
using Domain.Models.ProjectModel;
using Xbehave;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute

namespace Domain.SpecFlow.UnitTests
{
    public class ProjectNameTests
    {
        [ Scenario ]
        [ Example( null ) ]
        [ Example( "" ) ]
        [ Example( "    " ) ]
        public void CreateShouldFail( string projectNameValue )
        {
            Exception createException = null;

            $"Given: I have string \"{projectNameValue ?? "null"}\"".x( () => Assert.True( string.IsNullOrWhiteSpace( projectNameValue ) ) );
            "When I create ProjectName and give constructor string I have"
                .x( () => createException = Assert.Throws<ArgumentException>( () => new ProjectName( projectNameValue ) ) );
            "Then I should get ArgumentException exception".x( () => Assert.IsType<ArgumentException>( createException ) );
        }

        [ Scenario ]
        [ Example( "Project1" ) ]
        [ Example( "Project2" ) ]
        [ Example( "Prj3" ) ]
        public void Create( string projectNameValue )
        {
            ProjectName projectName = null;
            $"Given: I have not empty string {projectNameValue}".x( () =>
                                                                        Assert.True( ! string.IsNullOrWhiteSpace( projectNameValue ) ) );
            "When I create ProjectName".x( () => projectName = new ProjectName( projectNameValue ) );
            "Then I should have not null object with property Value equals to my string"
                .x( () =>
                {
                    Assert.NotNull( projectName );
                    Assert.Equal( projectNameValue, projectName.Value );
                } );
        }

        [ Scenario ]
        [ Example( "Project1" ) ]
        [ Example( "Project2" ) ]
        [ Example( "Prj3" ) ]
        public void Equality( string projectNameValue )
        {
            ProjectName p1            = null;
            ProjectName p2            = null;
            string      anotherString = null;
            $"Given: I have not empty string {projectNameValue}".x( () =>
                                                                        Assert.True( ! string.IsNullOrWhiteSpace( projectNameValue ) ) );
            "When I create two ProjectName object with my string"
                .x( () =>
                {
                    p1 = new ProjectName( projectNameValue );
                    p2 = new ProjectName( projectNameValue );
                } );
            "Then this two objects should be equal and have the same HashCode"
                .x( () =>
                {
                    Assert.Equal( p1, p2 );
                    Assert.True( p1 == p2 );
                    Assert.Equal( p1.GetHashCode(), p2.GetHashCode() );
                } );

            $"Given: I have another string {projectNameValue + " " + projectNameValue}"
                .x( () => anotherString = projectNameValue + " " + projectNameValue );

            "When I create ProjectName for both strings"
                .x( () =>
                {
                    p1 = new ProjectName( projectNameValue );
                    p2 = new ProjectName( anotherString );
                } );

            "Then this two objects should be not equal and should not have the same HashCode"
                .x( () =>
                {
                    Assert.NotEqual( p1, p2 );
                    Assert.True( p1 != p2 );
                    Assert.NotEqual( p1.GetHashCode(), p2.GetHashCode() );
                } );
        }
    }
}