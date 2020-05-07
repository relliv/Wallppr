using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Wallppr.Data;
using Wallppr.Helpers;
using Wallppr.Models.Common;
using static Wallppr.DI.DI;

namespace Wallppr.ViewModel.Wallpaper
{
    public class MyWallpapersViewModel : ViewModelBase
    {
        public MyWallpapersViewModel()
        {
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);
            ShowWallpaperCommand = new RelayParameterizedCommand(ShowWallpaper);
            SetAsCommand = new RelayParameterizedCommand(SetAs);
            SetFavoriteWallpaperCommand = new RelayParameterizedCommand(SetFavoriteWallpaper);

            Wallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();

            LoadWallpapers();
        }

        #region Commands

        public ICommand SetAsCommand { get; set; }
        public ICommand SetFavoriteWallpaperCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        public ICommand ShowWallpaperCommand { get; set; }

        #endregion


        #region Public Properties

        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> Wallpapers { get; set; }

        #endregion

        #region Pagination

        public Pagination Pagination { get; set; }
        public int PageLimit { get; set; } = 40;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Load saved wallpapers
        /// </summary>
        public void LoadWallpapers()
        {
            using var db = new AppDbContext();

            var totalSize = db.Wallpapers
            .Count();
            totalSize = totalSize > 0 ? totalSize : 1;

            Pagination = new Pagination(totalSize, CurrentPage, PageLimit, 10);

            Wallpapers = db.Wallpapers
            .Select(x => new Models.Wallpaper.Entities.Wallpaper
            {
                Id = x.Id,
                UId = x.UId,
                Path = x.Path,
                Thumbnail = x.Thumbnail,
                DimensionX = x.DimensionX,
                DimensionY = x.DimensionY,
                IsFavorite = x.IsFavorite,
                WallpaperUrl = x.WallpaperUrl,
                WallpaperThumbUrl = x.WallpaperThumbUrl,
                ColorPalette = db.Colors.Where(c => c.WallpaperId == x.Id)
                .ToObservableCollection()
            })
            .OrderBy(x => x.Id)
            .Skip((CurrentPage - 1) * PageLimit)
            .Take(PageLimit)
            .ToObservableCollection();

            Pagination = new Pagination(totalSize, CurrentPage, PageLimit, 10);
        }

        #endregion


        #region Command Methods

        /// <summary>
        /// Go to selected page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPage(object sender)
        {
            var page = (Models.Common.Page)sender;

            CurrentPage = page.PageNumber;
            LoadWallpapers();
        }

        /// <summary>
        /// Show wallpaper with orginal size
        /// </summary>
        /// <param name="sender"></param>
        public void ShowWallpaper(object sender)
        {
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)sender;

            ViewModelApplication.SelectedWallpaper = wallpaper;
            ViewModelApplication.TempWallpapers = Wallpapers;
            ViewModelApplication.TempPagination = Pagination;
            ViewModelApplication.BackToButtonVisibility = Visibility.Visible;
            ViewModelApplication.PreviousPage = ApplicationPage.MyWallpapers;

            ViewModelApplication.GoToPage(ApplicationPage.Wallpaper);
        }

        /// <summary>
        /// Apply set as commands
        /// </summary>
        /// <param name="wallpaper"></param>
        /// <param name="action"></param>
        public void SetAs(object parameter)
        {
            var values = (object[])parameter;
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)values[0];
            var action = (string)values[1];

            if (action == "AsBackground")
            {
                new Uri(wallpaper.Path).SetAsDesktopWallpaper(SetAsWallpaperHelpers.Style.Centered);
            }
            else if (action == "SaveAs")
            {
                var folderBrowserDialog = new FolderBrowserDialog
                {
                    Description = "Select Save Destination",
                    RootFolder = Environment.SpecialFolder.Desktop
                };

                var result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    var fileName = Path.GetFileName(wallpaper.Path);

                    File.Copy(wallpaper.Path, $"{folderBrowserDialog.SelectedPath}\\{fileName}");
                }
            }
            else if (action == "Remove")
            {
                if (!wallpaper.Path.StartsWith("https://"))
                    File.Delete(wallpaper.Path);

                if (!wallpaper.Thumbnail.BitmapImageToPath().StartsWith("https://"))
                    File.Delete(wallpaper.Thumbnail.BitmapImageToPath());

                using var db = new AppDbContext();
                db.Wallpapers.Remove(wallpaper);
                db.SaveChanges();

                Wallpapers.Remove(wallpaper);
            }
        }

        /// <summary>
        /// Add to favorites
        /// </summary>
        /// <param name="sender"></param>
        public void SetFavoriteWallpaper(object sender)
        {
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)sender;

            using var db = new AppDbContext();
            db.Wallpapers.Update(wallpaper);
            db.SaveChanges();
        }

        #endregion
    }
}