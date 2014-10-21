using SenseLab.Common.ViewModels;

namespace SenseLab
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            var shell = new ShellViewModel(frame.NavigationService);
            DataContext = shell;
        }
    }
}
