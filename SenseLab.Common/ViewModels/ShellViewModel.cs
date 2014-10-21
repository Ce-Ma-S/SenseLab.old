using SenseLab.Common.Commands;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SenseLab.Common.ViewModels
{
    public class ShellViewModel :
        ViewModel
    {
        public ShellViewModel(NavigationService navigationService)
        {
            navigationService.ValidateNonNull("navigationService");
            NavigationService = navigationService;
            NavigateCommand = new Command<object>(p => new Task(pp => Navigate(pp), p));
        }

        public NavigationService NavigationService { get; private set; }

        public void Navigate(object parameter)
        {
            var ended = new AutoResetEvent(false);
            NavigatedEventHandler navigated = (s, e) => ended.Set();
            NavigationService.Navigated += navigated;
            Application.Current.Dispatcher.BeginInvoke(new Action(
                () =>
                {
                    if (parameter is Uri)
                        NavigationService.Navigate((Uri)parameter);
                    else if (parameter is string)
                        NavigationService.Navigate(new Uri((string)parameter, UriKind.RelativeOrAbsolute));
                    else
                        NavigationService.Navigate(parameter);
                }));
            Task.Run(
                () =>
                {
                    ended.WaitOne();
                    NavigationService.Navigated -= navigated;
                })
                .Wait();
        }
        public ICommand NavigateCommand { get; private set; }
    }
}
