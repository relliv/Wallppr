/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:KOR.SysInfo.Core"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Wallppr.ViewModel.App;
using Wallppr.ViewModel.Wallpaper;
using static Wallppr.DI.DI;
//using Microsoft.Practices.ServiceLocation;

namespace Wallppr.ViewModel.Base
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {
            }
            else
            {
                SimpleIoc.Default.Register<NavbarViewModel>();
                SimpleIoc.Default.Register<AppSettingsViewModel>();
                SimpleIoc.Default.Register<MyWallpapersViewModel>();
                SimpleIoc.Default.Register<DesktopWallpapersViewModel>();
                SimpleIoc.Default.Register<MobileWallpapersViewModel>();
                SimpleIoc.Default.Register<HistoryWallpapersViewModel>();
            }
        }

        #region Public Properties

        /// <summary>
        /// Singleton instance of the locator
        /// </summary>
        public static ViewModelLocator Instance { get; private set; } = new ViewModelLocator();

        /// <summary>
        /// The application view model
        /// </summary>
        public ApplicationViewModel ApplicationViewModel => ViewModelApplication;

        #endregion

        public NavbarViewModel NavbarVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<NavbarViewModel>();
            }
        }

        public AppSettingsViewModel AppSettingsVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AppSettingsViewModel>();
            }
        }

        public MyWallpapersViewModel MyWallpapersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MyWallpapersViewModel>();
            }
        }

        public DesktopWallpapersViewModel DesktopWallpapersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DesktopWallpapersViewModel>();
            }
        }

        public MobileWallpapersViewModel MobileWallpapersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MobileWallpapersViewModel>();
            }
        }

        public HistoryWallpapersViewModel HistoryWallpapersVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HistoryWallpapersViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}