using SenseLab.Common.Commands;
using SenseLab.Common.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            NavigateCommand = new Command<object>((p, cts) => Navigate(p));
            if (CanNavigate(LastInternalNavigationUri))
                Navigate(LastInternalNavigationUri);
        }

        #region Navigation

        public NavigationService NavigationService { get; private set; }
        public Uri LastInternalNavigationUri
        {
            get { return Settings.Default.LastInternalNavigationUri; }
            set
            {
                SetProperty(() => LastInternalNavigationUri, v => Settings.Default.LastInternalNavigationUri = v, value);
            }
        }

        public bool CanNavigate(object parameter)
        {
            return parameter is string ?
                !string.IsNullOrEmpty((string)parameter) :
                parameter != null;
        }
        /// <summary>
        /// Navigates to <paramref name="parameter"/> synchronously.
        /// </summary>
        /// <param name="parameter">Uri or object (Page).</param>
        public async Task Navigate(object parameter)
        {
            var navigation = new TaskCompletionSource<object>();
            NavigationService.Navigated += OnNavigated;
            NavigationService.NavigationStopped += OnNavigationStopped;
            NavigationService.NavigationFailed += OnNavigationFailed;
            try
            {
                await Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        if (parameter is string)
                            parameter = new Uri((string)parameter, UriKind.RelativeOrAbsolute);
                        if (parameter is Uri)
                        {
                            var uri = (Uri)parameter;
                            // external
                            if (uri.IsAbsoluteUri && externalUriSchemes.Contains(uri.Scheme, StringComparer.OrdinalIgnoreCase))
                            {
                                try
                                {
                                    Process.Start(uri.AbsoluteUri);
                                    navigation.SetResult(null);
                                }
                                catch (Exception e)
                                {
                                    navigation.SetException(new Exception("Navigation failed.", e));
                                }
                            }
                            // internal
                            else
                            {
                                Page keptAlivePage;
                                if (uriToKeptAlivePage.TryGetValue(uri, out keptAlivePage))
                                    NavigationService.Navigate(keptAlivePage, navigation);
                                else
                                    NavigationService.Navigate(uri, navigation);
                            }
                        }
                        else
                            NavigationService.Navigate(parameter, navigation);
                    });
                await navigation.Task;
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
            var navigation = (TaskCompletionSource<object>)e.ExtraData;
            var uri = e.Uri;
            if (uri == null)
                uri = uriToKeptAlivePage.FirstOrDefault(i => i.Value == e.Content).Key;
            if (uri != null)
            {
                LastInternalNavigationUri = uri;
                if (e.Content is Page)
                {
                    var page = (Page)e.Content;
                    if (page.KeepAlive)
                        uriToKeptAlivePage[uri] = page;
                }
            }
            navigation.SetResult(null);
        }
        private void OnNavigationStopped(object sender, NavigationEventArgs e)
        {
            var navigation = (TaskCompletionSource<object>)e.ExtraData;
            navigation.SetCanceled();
        }
        private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            var navigation = (TaskCompletionSource<object>)e.ExtraData;
            e.Handled = true;
            navigation.SetException(new Exception("Navigation failed.", e.Exception));
        }

        private static readonly string[] externalUriSchemes = new string[] {
            Uri.UriSchemeHttp,
            Uri.UriSchemeHttps,
            Uri.UriSchemeMailto,
            Uri.UriSchemeFile
            };

        // TODO: use Prism navigation and remove view specific clases (Page, NavigationService) from this view model
        // to respect Page.KeepAlive in NavigationService.Navigate (it is respected only in NavigationService.NavigateBack/Forward etc.)
        private Dictionary<Uri, Page> uriToKeptAlivePage = new Dictionary<Uri,Page>();

        #endregion
    }
}
