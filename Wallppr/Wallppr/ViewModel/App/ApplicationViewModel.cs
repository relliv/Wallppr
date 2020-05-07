using GalaSoft.MvvmLight;
using Wallppr.Data;
using Wallppr.Models.Common;
using System.ComponentModel;
using Wallppr.ViewModel.Base;
using System.Windows;
using System.Collections.ObjectModel;
using Wallppr.Helpers;

namespace Wallppr.ViewModel.App
{
    /// <summary>
    /// The application state as a view model
    /// </summary>
    public class ApplicationViewModel : BaseViewModel
    {
        public ApplicationViewModel()
        {
            using var db = new AppDbContext();

            AppSettings = new AppSettings();

            BackToButtonVisibility = Visibility.Hidden;
        }

        #region Properties

        public ApplicationPage CurrentPage { get; private set; }
        public ApplicationPage PreviousPage { get; set; }
        public ViewModelBase CurrentPageViewModel { get; set; }
        public AppSettings AppSettings { get; set; }

        public Models.Wallpaper.Entities.Wallpaper SelectedWallpaper { get; set; }
        public Visibility BackToButtonVisibility { get; set; } = Visibility.Hidden;
        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> TempWallpapers { get; set; }
        public Pagination TempPagination { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Navigates to the specified page
        /// </summary>
        /// <param name="page">The page to go to</param>
        /// <param name="viewModel">The view model, if any, to set explicitly to the new page</param>
        public void GoToPage(ApplicationPage page, ViewModelBase viewModel = null)
        {
            CurrentPageViewModel = viewModel;

            var different = CurrentPage != page;

            CurrentPage = page;

            if (!different)
                OnPropertyChanged(nameof(CurrentPage));
        }

        #endregion
    }
}