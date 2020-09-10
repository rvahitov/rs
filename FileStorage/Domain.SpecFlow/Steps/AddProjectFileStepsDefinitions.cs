using System;
using System.IO;
using System.Text;
using Domain.Application;
using Domain.Models.ProjectModel;
using TechTalk.SpecFlow;
using Xunit;

namespace Domain.SpecFlow.Steps
{
    [ Binding ]
    public sealed class AddProjectFileStepsDefinitions
    {
        private readonly FileStorageApplication _application;
        private          ProjectName            _projectName;
        private          ProjectFolder          _projectFolder;
        private          string                 _fileContent;
        private          string                 _filePath;
        private          string                 _directoryPath;

        public AddProjectFileStepsDefinitions( FileStorageApplication application )
        {
            _application = application;
        }

        [ Given( "в системе есть проект (.*) и с папкой (.*)" ) ]
        public void SystemDoesHaveProject( string projectNameValue, string projectFolderPath )
        {
            _projectName = new ProjectName( projectNameValue );

            _projectFolder = new ProjectFolder( projectFolderPath );
            _application.CreateProject( _projectName, _projectFolder );
        }

        [ When( "я в проект добавляю файл с содержимым (.*)" ) ]
        public void WhenIAddFileWithContentToProject( string fileContent )
        {
            _fileContent = fileContent;
            var contentBytes = Encoding.UTF8.GetBytes( _fileContent );
            _application.AddProjectFile( _projectName, new ReadOnlyMemory<byte>( contentBytes ) );
        }


        [ Then( "в папке проекте должен появиться файл с именем (.*)" ) ]
        public void ThenProjectFolderShouldContainFile( string fileName )
        {
            _directoryPath = Path.GetFullPath( _projectFolder.Path );
            Assert.True( Directory.Exists( _directoryPath ) );
            _filePath = Path.Combine( _directoryPath, fileName );
            Assert.True( File.Exists( _filePath ) );
        }

        [ Then( "его содержимое должно соответсвовать  данному содержанию" ) ]
        public void AndFileContentShouldBeEqualToProvidedContent()
        {
            try
            {
                var fileBytes   = File.ReadAllBytes( _filePath );
                var fileContent = Encoding.UTF8.GetString( fileBytes );
                Assert.Equal( _fileContent, fileContent );
            }
            finally
            {
                Directory.Delete( _directoryPath, true );
            }
        }
    }
}