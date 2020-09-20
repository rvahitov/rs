using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Application;
using Domain.Models.ProjectModel;
using FluentAssertions;
using TechTalk.SpecFlow;
using Xunit;

namespace Domain.SpecFlow.Steps
{
    [Binding]
    public sealed class AddProjectFileStepsDefinitions
    {
        private readonly FileStorageApplication _application;
        private ProjectName _projectName;
        private ProjectFolder _projectFolder;
        private string _directoryPath;
        private List<byte[]> _contents;
        private Project _project;

        private readonly List<(string id, string content)> _files = new List<(string id, string content)>();

        public AddProjectFileStepsDefinitions(FileStorageApplication application)
        {
            _application = application;
        }

        [Given("в системе есть проект (.*) и с папкой (.*)")]
        public void SystemDoesHaveProject(string projectNameValue, string projectFolderPath)
        {
            _projectName = new ProjectName(projectNameValue);

            _projectFolder = new ProjectFolder(projectFolderPath);
            _application.CreateProject(_projectName, _projectFolder);
        }

        [Given("есть следующие файлы")]
        public void GivenSomeFiles(Table table)
        {
            _files.Clear();
            _files.AddRange(table.Rows.Select(r => (r[0], r[1])));
        }

        [When("я в проект добавляю эти файлы")]
        public async Task WhenIAddThisFilesToProject()
        {
            foreach (var contentBytes in from item in _files select Encoding.UTF8.GetBytes(item.content))
            {
                _application.AddProjectFile(_projectName, contentBytes);
            }

            await Task.Delay(TimeSpan.FromSeconds(3));
        }


        [Then("в папке проекте должны появиться эти файлы")]
        public void ThenProjectFolderShouldContainFile()
        {
            _directoryPath = Path.GetFullPath(_projectFolder.Path);
            Assert.True(Directory.Exists(_directoryPath));
            Assert.All(_files.Select(t => t.id), id =>
            {
                var filePath = Path.Combine(_directoryPath, $"{_projectName.Value}_{id}");
                Assert.True(File.Exists(filePath));
            });
        }

        [Then("их содержимое должно соответсвовать")]
        public void AndFileContentShouldBeEqualToProvidedContent()
        {
            try
            {
                Assert.All(_files, t =>
                {
                    var (id, content) = t;
                    var filePath = Path.Combine(_directoryPath, $"{_projectName.Value}_{id}");
                    var contentBytes = File.ReadAllBytes(filePath);
                    Assert.Equal(content, Encoding.UTF8.GetString(contentBytes));
                });
            }
            finally
            {
                Directory.Delete(_directoryPath, true);
            }
        }

        [Given("есть файлы с содержимым")]
        public void GivenFilesWithContent(Table contents)
        {
            var bytes =
                from contentRow in contents.Rows
                let parts = contentRow[0].Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries)
                let array = parts.Select(byte.Parse).ToArray()
                select array;
            _contents = bytes.ToList();
        }

        [When("я в проект добавляю файлы с содержимым")]
        public void WhenIAddContentToProject()
        {
            foreach (var content in _contents)
            {
                _application.AddProjectFile(_projectName, content);
            }
        }

        [When("запрашиваю проект")]
        public async Task WhenIAskForProject()
        {
            var result = await _application.GetProject(_projectName);
            result.IsSuccess.Should().BeTrue();
            _project = result.SuccessValue;
        }

        [Then("я должен получить проект с (\\d+) файлами")]
        public void ThenIShouldGetProjectWithFilesCount(int filesCount)
        {
            _project.Files.Should().HaveCount(filesCount);
        }

        [Then("общий размер файлов должен быть (\\d+) байт")]
        public void ThenIShouldGetProjectWithTotalFileSize(long totalFileSize)
        {
            _project.TotalFileSize.Should().Be(totalFileSize);
        }
    }
}