using SenseLab.Common.ViewModels;
using SenseLab.Pages;
using System.Windows.Controls;

namespace SenseLab
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitPages();
            InitializeComponent();
            var shell = new ShellViewModel(frame.NavigationService);
            DataContext = shell;
        }

        public Page EnvironmentPage { get; private set; }
        public Page ProjectsStoredPage { get; private set; }
        public Page ProjectsOpenPage { get; private set; }

        private void InitPages()
        {
            EnvironmentPage = new Environment();
            ProjectsStoredPage = new ProjectsStored();
            ProjectsOpenPage = new ProjectsOpen();
        }
    }
}
