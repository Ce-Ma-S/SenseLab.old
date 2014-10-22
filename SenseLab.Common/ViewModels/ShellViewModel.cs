using SenseLab.Common.Commands;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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
            NavigateCommand = new Command<object>((p, cts) => Navigate(p), false);
        }

        #region Navigation

        private class NavigationData
        {
            public NavigationData(CancellationTokenSource cancellation)
            {
                Cancellation = cancellation;
            }

            public readonly AutoResetEvent Ended = new AutoResetEvent(false);
            public readonly CancellationTokenSource Cancellation;
            public Exception Error;
        }

        public NavigationService NavigationService { get; private set; }

        /// <summary>
        /// Navigates to <paramref name="parameter"/> synchronously.
        /// </summary>
        /// <param name="parameter">Uri or object (Page).</param>
        /// <param name="cancellation">Cancellation.</param>
        public void Navigate(object parameter, CancellationTokenSource cancellation = null)
        {
            var navigation = new NavigationData(cancellation);
            NavigationService.Navigated += OnNavigated;
            NavigationService.NavigationStopped += OnNavigationStopped;
            NavigationService.NavigationFailed += OnNavigationFailed;
            try
            {
                Application.Current.Dispatcher.BeginInvoke(new Action(
                    () =>
                    {
                        if (parameter is string)
                            parameter = new Uri((string)parameter, UriKind.RelativeOrAbsolute);
                        if (parameter is Uri)
                        {
                            var uri = (Uri)parameter;
                            if (uri.IsAbsoluteUri && externalUriSchemes.Contains(uri.Scheme, StringComparer.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    Process.Start(uri.AbsoluteUri);
                                }
                                catch (Exception e)
                                {
                                    navigation.Error = e;
                                }
                                finally
                                {
                                    navigation.Ended.Set();
                                }
                            }
                            else
                                NavigationService.Navigate(uri, navigation);
                        }
                        else
                            NavigationService.Navigate(parameter, navigation);
                    }));
                navigation.Ended.WaitOne();
                if (navigation.Error != null)
                    throw new Exception("Navigation failed.", navigation.Error);
            }
            finally
            {
                NavigationService.Navigated -= OnNavigated;
                NavigationService.NavigationStopped -= OnNavigationStopped;
                NavigationService.NavigationFailed -= OnNavigationFailed;
            }
        }
        public ICommand NavigateCommand { get; private set; }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            var navigation = (NavigationData)e.ExtraData;
            navigation.Ended.Set();
        }
        private void OnNavigationStopped(object sender, NavigationEventArgs e)
        {
            var navigation = (NavigationData)e.ExtraData;
            if (navigation.Cancellation != null)
            {
                navigation.Cancellation.Cancel();
                navigation.Cancellation.Token.ThrowIfCancellationRequested();
            }
            navigation.Ended.Set();
        }
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            var navigation = (NavigationData)e.ExtraData;
            navigation.Error = e.Exception;
            e.Handled = true;
            navigation.Ended.Set();
        }

        private static readonly string[] externalUriSchemes = new string[] {
            Uri.UriSchemeHttp,
            Uri.UriSchemeHttps,
            Uri.UriSchemeMailto,
            Uri.UriSchemeFile
            };

        #endregion
    }
}
