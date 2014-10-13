using Microsoft.Practices.ServiceLocation;
using SenseLab.Common.Collections;
using SenseLab.Common.Data;
using SenseLab.Common.Environments;
using SenseLab.Common.Properties;
using SenseLab.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace SenseLab.Common.Projects
{
    public class ProjectsViewModel :
        ViewModel
    {
        public ProjectsViewModel()
        {
            storages = new ObservableCollectionEx<IProjectStorage, Guid>();
            storages.CollectionChanged += OnStoragesChanged;
            openProjects = new ObservableCollectionEx<IProject>();
            openProjects.CollectionChanged += OnOpenProjectsChanged;
            Environment = ServiceLocator.Current.GetInstance<IEnvironment>();
            InitStorages();
        }

        public IList<IProjectStorage> Storages
        {
            get { return storages; }
        }
        public IProjectStorage SelectedStorage
        {
            get { return selectedStorage; }
            set
            {
                SetProperty(() => SelectedStorage, ref selectedStorage, value,
                    beforeChange: (n, v) =>
                    {
                        if (value != null)
                            Storages.ValidateContainmentOf(value);
                    });
            }
        }
        public IList<IProject> OpenProjects
        {
            get { return openProjects; }
        }
        public IProject SelectedOpenProject
        {
            get { return selectedOpenProject; }
            set
            {
                SetProperty(() => SelectedOpenProject, ref selectedOpenProject, value,
                    beforeChange: (n, v) =>
                    {
                        if (value != null)
                            OpenProjects.ValidateContainmentOf(value);
                    });
            }
        }
        public IEnvironment Environment { get; private set; }

        public bool CanCreateProject
        {
            get { return SelectedStorageWritableAndConnected; }
        }
        public async Task<IProject> CreateProject()
        {
            var storage = SelectedStorage;
            var projectId = Guid.NewGuid();
            var project = new Project(projectId, "Project",
                await storage.CreateRecordStorage(projectId));
            await SelectedStorage.Save(project);
            OpenProject(project, storage);
            return project;
        }

        public async Task<bool> CanOpenProject(Guid projectId, Guid storageId)
        {
            IProjectStorage storage;
            if (!storages.TryGetItem(storageId, out storage))
                return false;
            return GetOpenProject(projectId, storageId) == null &&
                storage.IsConnected && await storage.Contains(projectId);
        }
        public async Task<IProject> OpenProject(Guid projectId, Guid storageId)
        {
            var storage = storages.GetItem(storageId);
            var project = await storage.FirstOrDefault(projectId);
            OpenProject(project, storage);
            return project;
        }

        public bool CanCloseProject(IProject project)
        {
            return project != null && OpenProjects.Contains(project);
        }
        public async Task CloseProject(IProject project)
        {
            if (((IChangeAware)project).IsChanged)
            {
                var save = AskToSaveProjectOnClose(project);
                if (!save.HasValue)
                    return;
                if (save.Value && !await SaveProject(project))
                    return;
            }
            OpenProjects.Remove(project);
            openProjectToStorage.Remove(project);
        }

        public async Task<bool> CanSaveProject(IProject project)
        {
            if (project == null)
                return false;
            var storage = openProjectToStorage[project];
            return ((IChangeAware)project).IsChanged &&
                storage.IsWritableAndConnected() && await storage.Contains(project.Id);
        }
        public async Task<bool> SaveProject(IProject project)
        {
            var storage = openProjectToStorage[project];
            if (storage.IsWritableAndConnected())
            {
                await storage.Save(project);
                ((IChangeAware)project).IsChanged = false;
                return true;
            }
            else
                return await SaveProjectAs(project);
        }

        public bool CanSaveProjectAs(IProject project)
        {
            return project != null && Storages.Where(s => s.IsWritableAndConnected()).Any();
        }
        public async Task<bool> SaveProjectAs(IProject project)
        {
            var storages = Storages.Where(s => s.IsWritableAndConnected()).ToArray();
            if (!storages.Any())
                return false;
            var storage = AskToSaveProjectAs(project, storages);
            if (storage == null)
            {
                return false;
            }
            return await SaveProjectAs(project, storage);
        }
        public async Task<bool> SaveProjectAs(IProject project, IProjectStorage storage)
        {
            if (await storage.Contains(project.Id))
            {
                switch (AskToSaveProjectAsUpdateOrClone(project, storage))
                {
                    case SaveProjectAsUpdateOrCloneModes.Update:
                        await storage.Save(project);
                        break;
                    case SaveProjectAsUpdateOrCloneModes.CloneWithRecords:
                        await SaveProjectAsClone(project, storage, true);
                        break;
                    case SaveProjectAsUpdateOrCloneModes.CloneWithoutRecords:
                        await SaveProjectAsClone(project, storage, false);
                        break;
                    case SaveProjectAsUpdateOrCloneModes.Cancel:
                    default:
                        return false;
                }
            }
            else
                await storage.Save(project);
            ((IChangeAware)project).IsChanged = false;
            return true;
        }
        public bool CanSaveProjectAsClone(IProject project, IProjectStorage storage)
        {
            return project != null && storage.IsWritableAndConnected();
        }
        public async Task SaveProjectAsClone(IProject project, IProjectStorage storage, bool copyRecords)
        {
            var clone = await project.Clone(async cloneId => await storage.CreateRecordStorage(cloneId));
            await storage.Save(clone);
            if (copyRecords)
                await project.Records.CopyTo(clone.Records);
        }

        public async Task<bool> CanRemoveProject(Guid projectId, Guid storageId)
        {
            IProjectStorage storage;
            return storages.TryGetItem(storageId, out storage) &&
                storage.IsWritableAndConnected() &&
                await SelectedStorage.Contains(projectId);
        }
        public async Task<bool> RemoveProject(Guid projectId, Guid storageId)
        {
            var storage = storages.GetItem(storageId);
            if (!AskToRemoveProject(projectId, storage))
                return false;
            await storage.Remove(projectId);
            var openProject = GetOpenProject(projectId, storageId);
            if (openProject != null)
            {
                ((IChangeAware)openProject).IsChanged = false;
                await CloseProject(openProject);
            }
            return true;
        }

        #region Questions

        public Func<IProject, bool?> AskToSaveProjectOnCloseMethod;
        public Func<IProject, IEnumerable<IProjectStorage>, IProjectStorage> AskToSaveProjectAsMethod;
        public Func<IProject, IProjectStorage, SaveProjectAsUpdateOrCloneModes> AskToSaveProjectAsUpdateOrCloneMethod;
        public Func<Guid, IProjectStorage, bool> AskToRemoveProjectMethod;

        private bool? AskToSaveProjectOnClose(IProject project)
        {
            if (AskToSaveProjectOnCloseMethod != null)
                return AskToSaveProjectOnCloseMethod(project);
            return true;
        }
        private IProjectStorage AskToSaveProjectAs(IProject project, IEnumerable<IProjectStorage> storages)
        {
            IProjectStorage storage;
            if (AskToSaveProjectAsMethod != null)
                storage = AskToSaveProjectAsMethod(project, storages);
            else
                storage = storages.FirstOrDefault();
            return storage;
        }
        private SaveProjectAsUpdateOrCloneModes AskToSaveProjectAsUpdateOrClone(IProject project, IProjectStorage storage)
        {
            if (AskToSaveProjectAsUpdateOrCloneMethod != null)
                return AskToSaveProjectAsUpdateOrCloneMethod(project, storage);
            return SaveProjectAsUpdateOrCloneModes.Update;
        }
        private bool AskToRemoveProject(Guid projectId, IProjectStorage storage)
        {
            if (AskToRemoveProjectMethod != null)
                return AskToRemoveProjectMethod(projectId, storage);
            return true;
        }

        #endregion

        private bool SelectedStorageWritableAndConnected
        {
            get { return SelectedStorage.IsWritableAndConnected(); }
        }

        private void InitStorages()
        {
            var projectFolders = Settings.Default.ProjectFolders.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var projectFolder in projectFolders)
            {
                var storage = new ProjectFileSerializingStorage(Guid.NewGuid(), "Projects", "Projects in files in a folder.", false, projectFolder);
                Storages.Add(storage);
            }
            openProjectToStorage = new Dictionary<IProject, IProjectStorage>();
        }
        private void OpenProject(IProject project, IProjectStorage storage)
        {
            OpenProjects.Add(project);
            SelectedOpenProject = project;
            openProjectToStorage.Add(project, storage);
        }
        private IProject GetOpenProject(Guid projectId, Guid storageId)
        {
            return openProjectToStorage.FirstOrDefault(item => item.Key.Id == projectId && item.Value.Id == storageId).Key;
        }

        private void OnStoragesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedStorage == null)
                SelectedStorage = Storages.FirstOrDefault();
            else if (!Storages.Contains(SelectedStorage))
                SelectedStorage = null;
        }
        private void OnOpenProjectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SelectedOpenProject == null)
                SelectedOpenProject = OpenProjects.FirstOrDefault();
            else if (!OpenProjects.Contains(SelectedOpenProject))
                SelectedOpenProject = null;
        }

        private ObservableCollectionEx<IProjectStorage, Guid> storages;
        private IProjectStorage selectedStorage;
        private ObservableCollectionEx<IProject> openProjects;
        private IProject selectedOpenProject;
        private Dictionary<IProject, IProjectStorage> openProjectToStorage;
    }
}
