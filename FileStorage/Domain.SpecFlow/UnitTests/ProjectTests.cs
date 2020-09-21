using System;
using Domain.Models.ProjectModel;
using FluentAssertions;
using Xunit;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable ObjectCreationAsStatement

namespace Domain.SpecFlow.UnitTests
{
    public sealed class ProjectTests
    {
        [ Fact ]
        public void CreateProjectShouldThrow()
        {
            Action create1 = () => new Project( null, new ProjectFolder( "a" ), Array.Empty<int>(), 10 );
            create1.Should().Throw<ArgumentNullException>()
                   .Which.ParamName.Should().Be( "name" );

            Action create2 = () => new Project( new ProjectName( "x" ), null, Array.Empty<int>(), 10 );
            create2.Should().Throw<ArgumentNullException>()
                   .Which.ParamName.Should().Be( "folder" );

            Action create3 = () => new Project( new ProjectName( "x" ), new ProjectFolder( "a" ), null, 10 );
            create3.Should().Throw<ArgumentNullException>()
                   .Which.ParamName.Should().Be( "files" );

            Action create4 = () => new Project( new ProjectName( "x" ), new ProjectFolder( "a" ), Array.Empty<int>(), -10 );
            create4.Should().Throw<ArgumentOutOfRangeException>()
                   .Which.ParamName.Should().Be( "totalFileSize" );

        }
    }
}