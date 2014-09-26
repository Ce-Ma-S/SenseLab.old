using SenseLab.Common.Environments;
using SenseLab.Common.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace SenseLab.Common.Projects
{
    public class ProjectsViewModel :
        ViewModel
    {
        public ProjectsViewModel()
        {
            var projects = new ObservableCollection<IProjects>();
            projects.CollectionChanged += OnProjectsChanged;
            Projects = projects;
            OpenProjects = new ObservableCollection<IProject>();
        }

        public IList<IProjects> Projects { get; private set; }
        public IProjects DefaultProjects
        {
            get { return defaultProjects; }
            set
            {
                if (value != null && !Projects.Contains(value))
                    throw new ArgumentOutOfRangeException();
                SetProperty(() => DefaultProjects, ref defaultProjects, value);
            }
        }
        public IList<IProject> OpenProjects { get; private set; }
        public EnvironmentsViewModel Environments { get; private set; }

        private void OnProjectsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (DefaultProjects != null && !Projects.Contains(DefaultProjects))
                DefaultProjects = null;
        }

        private IProjects defaultProjects;
    }
}
