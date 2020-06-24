using GalaSoft.MvvmLight;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using Wallppr.Data;
using Wallppr.Helpers;
using Wallppr.Models.Common;
using Wallppr.Models.Wallpaper.Enums;
using static Wallppr.DI.DI;

namespace Wallppr.ViewModel.Wallpaper
{
    public class HistoryWallpapersViewModel : WallpapersCommonViewModel
    {
        public HistoryWallpapersViewModel()
        {
            TabSelectionChangedCommand = new RelayParameterizedCommand(TabSelectionChanged);
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);
            ShowWallpaperCommand = new RelayParameterizedCommand(ShowWallpaper);
            SetAsCommand = new RelayParameterizedCommand(SetAs);
            SetFavoriteWallpaperCommand = new RelayParameterizedCommand(SetFavoriteWallpaper);

            DesktopWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();
            MobileWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();

            LoadDesktopWallpapers();
        }

        #region Commands

        public ICommand SetAsCommand { get; set; }
        public ICommand SetFavoriteWallpaperCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        public ICommand ShowWallpaperCommand { get; set; }
        public ICommand TabSelectionChangedCommand { get; set; }

        #endregion


        #region Public Properties

        #region Desktop Wallpapers

        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> DesktopWallpapers { get; set; }
        public Visibility DesktopWallpapersLIVisibility { get; set; }
        public Models.Wallpaper.Entities.Wallpaper SelectedDesktopWallpaper { get; set; }

        #endregion

        #region Mobile Wallpapers

        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> MobileWallpapers { get; set; }
        public Visibility MobileWallpapersLIVisibility { get; set; }
        public Models.Wallpaper.Entities.Wallpaper SelectedMobileWallpaper { get; set; }


        #endregion

        #endregion

        #region Desktop Pagination

        public Pagination DesktopPagination { get; set; }
        public int DesktopPageLimit { get; set; } = 40;
        public int DesktopCurrentPage { get; set; } = 1;
        public string DesktopSearchTerm { get; set; }

        #endregion

        #region Mobile Pagination

        public Pagination MobilePagination { get; set; }
        public int MobilePageLimit { get; set; } = 40;
        public int MobileCurrentPage { get; set; } = 1;
        public string MobileSearchTerm { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Load saved desktop wallpapers
        /// </summary>
        public void LoadDesktopWallpapers()
        {
            new Task(async () =>
            {
                DesktopWallpapersLIVisibility = Visibility.Visible;

                await Task.Delay(700);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    using var db = new AppDbContext();

                    var totalSize = db.Wallpapers
                    .Where(x => x.WallpaperType == WallpaperType.Desktop)
                    .Count();

                    totalSize = totalSize > 0 ? totalSize : 1;

                    DesktopPagination = new Pagination(totalSize, DesktopCurrentPage, DesktopPageLimit, 10);

                    DesktopWallpapers = db.Wallpapers
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
                        WallpaperType = x.WallpaperType,
                        ColorPalette = db.Colors.Where(c => c.WallpaperId == x.Id)
                        .ToObservableCollection(),
                        History = db.History.Where(c => c.WallpaperId == x.Id).ToObservableCollection()
                    })
                    .Where(x => x.WallpaperType == WallpaperType.Desktop && db.History.Any(c => c.WallpaperId == x.Id))
                    .OrderByDescending(x => x.AddedDate)
                    .Skip((DesktopCurrentPage - 1) * DesktopPageLimit)
                    .Take(DesktopPageLimit)
                    .ToObservableCollection();

                    SelectedDesktopWallpaper = DesktopWallpapers.FirstOrDefault();

                    DesktopPagination = new Pagination(totalSize, DesktopCurrentPage, DesktopPageLimit, 10);

                    DesktopWallpapersLIVisibility = Visibility.Hidden;
                });
            }).Start();
        }

        /// <summary>
        /// Load saved mobile wallpapers
        /// </summary>
        public void LoadMobileWallpapers()
        {
            new Task(async () =>
            {
                MobileWallpapersLIVisibility = Visibility.Visible;

                await Task.Delay(700);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    using var db = new AppDbContext();

                    var totalSize = db.Wallpapers
                    .Where(x => x.WallpaperType == WallpaperType.Mobile)
                    .Count();

                    totalSize = totalSize > 0 ? totalSize : 1;

                    MobilePagination = new Pagination(totalSize, MobileCurrentPage, MobilePageLimit, 10);

                    MobileWallpapers = db.Wallpapers
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
                        WallpaperType = x.WallpaperType,
                        ColorPalette = db.Colors.Where(c => c.WallpaperId == x.Id)
                        .ToObservableCollection(),
                        History = db.History.Where(c => c.WallpaperId == x.Id).ToObservableCollection()
                    })
                    .Where(x => x.WallpaperType == WallpaperType.Mobile && db.History.Any(c => c.WallpaperId == x.Id))
                    .OrderByDescending(x => x.AddedDate)
                    .Skip((MobileCurrentPage - 1) * MobilePageLimit)
                    .Take(MobilePageLimit)
                    .ToObservableCollection();

                    SelectedMobileWallpaper = MobileWallpapers.FirstOrDefault();

                    MobilePagination = new Pagination(totalSize, MobileCurrentPage, MobilePageLimit, 10);

                    MobileWallpapersLIVisibility = Visibility.Hidden;
                });
            }).Start();
        }
        #endregion


        #region Command Methods

        /// <summary>
        /// Tab selection changed
        /// </summary>
        /// <param name="sender"></param>
        public void TabSelectionChanged(object sender)
        {
            var tabIndex = (int)sender;

            if (tabIndex == 1 && (MobileWallpapers == null || MobileWallpapers.Count <= 0))
            {
                LoadMobileWallpapers();
            }
        }

        /// <summary>
        /// Go to selected page for desktop wallpapers
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPage(object sender)
        {
            var page = (Models.Common.Page)sender;

            DesktopCurrentPage = page.PageNumber;
            LoadDesktopWallpapers();
        }

        /// <summary>
        /// Show wallpaper with orginal size
        /// </summary>
        /// <param name="sender"></param>
        public void ShowWallpaper(object sender)
        {
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)sender;

            ViewModelApplication.SelectedWallpaper = wallpaper;
            //ViewModelApplication.TempWallpapers = Wallpapers;
            //ViewModelApplication.TempPagination = DesktopPagination;
            ViewModelApplication.BackToButtonVisibility = Visibility.Visible;
            ViewModelApplication.PreviousPage = ApplicationPage.History;

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
                    Description = ViewModelApplication.LanguageResourceDictionary["SelectSaveDestination"].ToString(),
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

                if (wallpaper.WallpaperType == WallpaperType.Desktop)
                {
                    DesktopWallpapers.Remove(wallpaper);
                }
                else if (wallpaper.WallpaperType == WallpaperType.Mobile)
                {
                    MobileWallpapers.Remove(wallpaper);
                }
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