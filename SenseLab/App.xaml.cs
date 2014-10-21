using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace SenseLab
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DispatcherUnhandledException += OnUnhandledException;
            AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        }

        private bool ProcessException(Exception exception)
        {
            var title = "Error ocured";
            var message = string.Format("The following problem ocured.\nClick Ok to continue, but continuing is unpredictable.\n\n{0}", exception);
            //var window = MainWindow as MetroWindow;
            //if (window != null)
            //{
            //    return window.ShowMessageAsync(title, message, MessageDialogStyle.AffirmativeAndNegative,
            //        new MetroDialogSettings()
            //        {
            //            AffirmativeButtonText = "Continue",
            //            NegativeButtonText = "Close"
            //        })
            //        .Result == MessageDialogResult.Affirmative;
            //}
            return MessageBox.Show(message, title, MessageBoxButton.OKCancel, MessageBoxImage.Error, MessageBoxResult.Cancel) == MessageBoxResult.OK;
        }

        private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = ProcessException(e.Exception);
        }
        private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception &&
                !ProcessException((Exception)e.ExceptionObject))
            {
                Process.GetCurrentProcess().Kill();
            }
        }
    }
}
