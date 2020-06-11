using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using Wallppr.Models.Common;
using static Wallppr.DI.DI;

namespace Wallppr.ViewModel.App
{
    public class NavbarViewModel : ViewModelBase
    {
        public NavbarViewModel()
        {
            NavbarItems = new ObservableCollection<NavbarItem>()
            {
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.DesktopWallpapers,
                    Title = ViewModelApplication.LanguageResourceDictionary["DesktopWallpapers"].ToString(),
                    IconData = (Application.Current.FindResource("Wallpaper") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.MobileWallpapers,
                    Title = ViewModelApplication.LanguageResourceDictionary["MobileWallpapers"].ToString(),
                    IconData = (Application.Current.FindResource("SmartCardOutline") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.MyWallpapers,
                    Title = ViewModelApplication.LanguageResourceDictionary["MyWallpapers"].ToString(),
                    IconData = (Application.Current.FindResource("HeartOutline") as Path)?.Data,
                }
            };

            SetIsChecked(ViewModelApplication.CurrentPage);

            GoToCommand = new RelayParameterizedCommand(GoTo);
            GoToPreviousPageCommand = new RelayCommand(p => GoToPreviousPage());
            GoToSettingsPageCommand = new RelayCommand(p => GoToSettingsPage());
        }

        #region Commands

        public ICommand GoToCommand { get; set; }
        public ICommand GoToPreviousPageCommand { get; set; }
        public ICommand GoToSettingsPageCommand { get; set; }

        #endregion


        #region Public Properties

        public ObservableCollection<NavbarItem> NavbarItems { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Go to app page
        /// </summary>
        /// <param name="sender"></param>
        public void GoTo(object sender)
        {
            var navbarItem = (NavbarItem)sender;

            if (ViewModelApplication.CurrentPage != navbarItem.ApplicationPage)
            {
                ViewModelApplication.TempWallpapers = null;
                ViewModelApplication.TempPagination = null;
                ViewModelApplication.BackToButtonVisibility = Visibility.Hidden;

                SetIsChecked(navbarItem.ApplicationPage);
                ViewModelApplication.GoToPage(navbarItem.ApplicationPage);
            }
        }

        /// <summary>
        /// Go to previous app page
        /// </summary>
        public void GoToPreviousPage()
        {
            SetIsChecked(ViewModelApplication.PreviousPage);
            ViewModelApplication.GoToPage(ViewModelApplication.PreviousPage);
        }

        /// <summary>
        /// Set is checked navbar item
        /// </summary>
        /// <param name="applicationPage"></param>
        public void SetIsChecked(ApplicationPage applicationPage)
        {
            foreach (var item in NavbarItems)
            {
                item.IsChecked = applicationPage == item.ApplicationPage ? true : false;
            }
        }

        /// <summary>
        /// Go to settings page
        /// </summary>
        public void GoToSettingsPage()
        {
            ViewModelApplication.GoToPage(ApplicationPage.AppSettings);
        }

        #endregion
    }
}