using Dna;
using Wallppr.DI;
using Wallppr.Models.Common;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Wallppr.Data;
using Wallppr.Dialogs;
using Wallppr.ViewModel;
using static Wallppr.DI.DI;
using System.Diagnostics;
using Wallppr.UI.Controls;
using Wallppr.ViewModel.App;

namespace Wallppr
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Current.DispatcherUnhandledException += Application_DispatcherUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _ = new AppDbContext();
            _ = new Setup();

            var splashScreen = new UI.Controls.SplashScreen();
            splashScreen.Show();

            base.OnStartup(e);

            ApplicationSetup();

            ViewModelApplication.GoToPage(ApplicationPage.DesktopWallpapers);

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Loaded += (s, e) =>
            {
                splashScreen.Close();
            };
            Current.MainWindow.DataContext = new WindowViewModel(Current.MainWindow);
            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            Framework.Construct<DefaultFrameworkConstruction>()
                .AddFileLogger()
                .AddAppViewModels()
                .Build();
        }

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: Application_DispatcherUnhandledException", args.Exception.ToString()));

            Debug.WriteLine(args.Exception.ToString(), "Error: Application_DispatcherUnhandledException");

            args.Handled = true;
        }

        private void TaskSchedulerOnUnobservedTaskException(UnobservedTaskExceptionEventArgs args)
        {
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: TaskSchedulerOnUnobservedTaskException", args.Exception.ToString()));

            Debug.WriteLine(args.Exception.ToString(), "Error: TaskSchedulerOnUnobservedTaskException");
        }

        private void CurrentOnDispatcherUnhandledException(DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(dialog, "Error: CurrentOnDispatcherUnhandledException", args.Exception.ToString()));

            Debug.WriteLine(args.Exception.ToString(), "Error: CurrentOnDispatcherUnhandledException");

            args.Handled = true;
        }
    }
}