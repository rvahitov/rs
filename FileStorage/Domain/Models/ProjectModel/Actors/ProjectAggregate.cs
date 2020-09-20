using System.Collections.Immutable;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.Persistence;
using Common.ExecutionResults;
using Domain.Models.ProjectModel.Actors.Events;
using Domain.Models.ProjectModel.Commands;
using Domain.Models.ProjectModel.Infrastructure;
using Domain.Models.ProjectModel.Queries;

namespace Domain.Models.ProjectModel.Actors
{
    public sealed class ProjectAggregate : ReceivePersistentActor
    {
        private int _currentFileId;
        private ImmutableList<int> _files = ImmutableList<int>.Empty;
        private bool _isNew = true;
        private ProjectFolder? _projectFolder;

        private ProjectName? _projectName;
        private long _totalFileSize;


        public ProjectAggregate(string persistenceId)
        {
            PersistenceId = persistenceId;

            Command<CreateProject>(OnCreateProject);

            Command<GetProject>(_ =>
            {
                if (_isNew)
                {
                    Sender.Tell(ExecutionResult.Success<Project?>(null), Self);
                    return;
                }

                var project = new Project(_projectName!, _projectFolder!, _files, _totalFileSize);

                Sender.Tell(ExecutionResult.Success<Project?>(project), Self);
            });

            Command<AddProjectFile>(OnAddProjectFile);

            CommandAny(cmd =>
            {
                Sender.Tell(CommonFailures.UnknownMessage);
                Unhandled(cmd);
            });
        }

        public override string PersistenceId { get; }

        private void OnAddProjectFile(AddProjectFile cmd)
        {
            if (_isNew)
            {
                Sender.Tell(ExecutionResult.Failed("Project does not exist"), Self);
                return;
            }

            var fileId = _currentFileId + 1;

            var directoryPah = Path.GetFullPath(_projectFolder!.Path);
            if (!Directory.Exists(directoryPah)) Directory.CreateDirectory(directoryPah);
            var filePath = Path.Combine(directoryPah, $"{_projectName!.Value}_{fileId}");
            File.WriteAllBytes(filePath, cmd.FileContent.ToArray());

            Persist(new ProjectFileAdded(fileId, cmd.FileContent.Length), e =>
            {
                _currentFileId = e.FileId;
                _files = _files.Add(e.FileId);
                _totalFileSize += e.FileSize;
                Sender.Tell(ExecutionResult.Success(), Self);
            });
        }

        private void OnCreateProject(CreateProject cmd)
        {
            if (!_isNew)
            {
                Sender.Tell(ExecutionResult.Failed(""), Self);
                return;
            }

            Persist(new ProjectCreated(cmd.ProjectName, cmd.ProjectFolder), e =>
            {
                _isNew = false;
                _projectName = e.ProjectName;
                _projectFolder = e.ProjectFolder;
                Sender.Tell(ExecutionResult.Success(), Self);
            });
        }
    }
}