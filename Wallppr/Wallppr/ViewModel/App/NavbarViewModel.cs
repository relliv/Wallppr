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
                    Title = "Desktop Wallpapers",
                    IconData = (Application.Current.FindResource("Wallpaper") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.MobileWallpapers,
                    Title = "Mobile Wallpapers",
                    IconData = (Application.Current.FindResource("SmartCardOutline") as Path)?.Data,
                },
                new NavbarItem()
                {
                    ApplicationPage = ApplicationPage.MyWallpapers,
                    Title = "My Wallpapers",
                    IconData = (Application.Current.FindResource("HeartOutline") as Path)?.Data,
                }
            };

            SetIsChecked(ViewModelApplication.CurrentPage);

            GoToCommand = new RelayParameterizedCommand(GoTo);
            GoToPreviousPageCommand = new RelayCommand(p => GoToPreviousPage());
        }

        #region Commands

        public ICommand GoToCommand { get; set; }
        public ICommand GoToPreviousPageCommand { get; set; }

        #endregion


        #region Public Properties

        public ObservableCollection<NavbarItem> NavbarItems { get; set; }

        #endregion


        #region Methods

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

        public void GoToPreviousPage()
        {
            SetIsChecked(ViewModelApplication.PreviousPage);
            ViewModelApplication.GoToPage(ViewModelApplication.PreviousPage);
        }

        public void SetIsChecked(ApplicationPage applicationPage)
        {
            foreach (var item in NavbarItems)
            {
                item.IsChecked = applicationPage == item.ApplicationPage ? true : false;
            }
        }

        #endregion
    }
}