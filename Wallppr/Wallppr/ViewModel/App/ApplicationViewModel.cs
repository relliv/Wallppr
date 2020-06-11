using GalaSoft.MvvmLight;
using Wallppr.Data;
using Wallppr.Models.Common;
using System.ComponentModel;
using Wallppr.ViewModel.Base;
using System.Windows;
using System.Collections.ObjectModel;
using Wallppr.Helpers;
using Wallppr.UI.i18N;
using KOR.ReleaseCheck;
using System.Linq;
using System;
using System.Diagnostics;

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

            LanguageResourceDictionary = i18N.GetCurrentLanguage();

            CheckAppVersion();
        }

        #region Properties

        public ApplicationPage CurrentPage { get; private set; }
        public ApplicationPage PreviousPage { get; set; }
        public ViewModelBase CurrentPageViewModel { get; set; }
        public AppSettings AppSettings { get; set; }

        public Models.Wallpaper.Entities.Wallpaper SelectedWallpaper { get; set; }
        public Visibility BackToButtonVisibility { get; set; } = Visibility.Hidden;

        public ResourceDictionary LanguageResourceDictionary { get; set; }


        #endregion


        #region Methods

        public void CheckAppVersion()
        {
            try
            {
                ReleaseCheck.GithubUsername = "EgoistDeveloper";
                ReleaseCheck.GithubRepo = "Wallppr";
                ReleaseCheck.UserAgentHeader = "Wallppr App";

                var releases = ReleaseCheck.GetReleases();

                if (releases.Count > 0)
                {
                    var lastRelease = releases.First();
                    var lastVersion = lastRelease.TagName.Replace("v", null);

                    var version1 = new Version(Settings.AppVersion);
                    var version2 = new Version(lastVersion);

                    if (version1.CompareTo(version2) < 0)
                    {
                        if (MessageBox.Show(
                            LanguageResourceDictionary["ThereIsNewerVersion"].ToString(), 
                            LanguageResourceDictionary["Update"].ToString(), 
                            MessageBoxButton.YesNo, 
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            Process.Start(lastRelease.HtmlUrl.OriginalString);
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                Debug.WriteLine(exp);
            }
        }


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