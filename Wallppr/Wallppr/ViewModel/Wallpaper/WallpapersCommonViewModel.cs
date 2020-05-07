using ColorThiefDotNet;
using GalaSoft.MvvmLight;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wallppr.Data;
using Wallppr.Helpers;
using Wallppr.Models.AppSetting;
using Wallppr.Models.Common;
using Wallppr.Models.Wallpaper;
using Wallppr.Models.Wallpaper.Enums;
using Wallppr.Models.Wallpaper.Json;
using static Wallppr.DI.DI;
using static Wallppr.Helpers.GetResolutions;

namespace Wallppr.ViewModel.Wallpaper
{
    public class WallpapersCommonViewModel : ViewModelBase
    {
        public WallpapersCommonViewModel()
        {
            GoToPageCommand = new RelayParameterizedCommand(GoToPage);
            ResolutionChangedCommand = new RelayParameterizedCommand(ResolutionChanged);
            ShowWallpaperCommand = new RelayParameterizedCommand(ShowWallpaper);
            SetAsCommand = new RelayParameterizedCommand(SetAs);
            SetFavoriteWallpaperCommand = new RelayParameterizedCommand(SetFavoriteWallpaper);

            Wallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();
            ProgressVisibility = Visibility.Hidden;
        }

        #region Commands

        public ICommand SetAsCommand { get; set; }
        public ICommand SetFavoriteWallpaperCommand { get; set; }
        public ICommand GoToPageCommand { get; set; }
        public ICommand ResolutionChangedCommand { get; set; }
        public ICommand ShowWallpaperCommand { get; set; }

        #endregion


        #region Public Properties

        public string ApiUrl { get; set; }
        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> Wallpapers { get; set; }
        public List<ResolutionRatio> ResolutionRatios { get; set; }
        public ResolutionRatio SelectedResolutionRatio { get; set; }
        public Models.Wallpaper.Resolution SelectedResolution { get; set; }


        public Models.Wallpaper.Entities.Wallpaper SelectedWallpaper { get; set; }
        public WallpaperType WallpapersType { get; set; }
        public double DownloadProgress { get; set; }
        public Visibility ProgressVisibility { get; set; }

        #endregion

        #region Pagination

        public Pagination Pagination { get; set; }
        public int PageLimit { get; set; } = 24;
        public int CurrentPage { get; set; } = 1;
        public string SearchTerm { get; set; }

        #endregion


        #region Methods

        /// <summary>
        /// Fetch and load wallpapers from apis
        /// </summary>
        public void LoadWallpapers()
        {
            new Task(() =>
            {
                Task.Delay(500);

                if (ViewModelApplication.TempWallpapers != null)
                {
                    Wallpapers = ViewModelApplication.TempWallpapers;
                    Pagination = ViewModelApplication.TempPagination;

                    ViewModelApplication.TempWallpapers = null;
                    ViewModelApplication.TempPagination = null;
                    ViewModelApplication.BackToButtonVisibility = Visibility.Hidden;

                    return;
                }

                SetApiPath();

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    var wallpapersData = ApiUrl.Get();

                    // load desktop wallpapers
                    if (WallpapersType == WallpaperType.Desktop)
                    {
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<WallpapersJson>(wallpapersData);

                        var wallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();
                        using var db = new AppDbContext();

                        foreach (var item in jsonObjwallpapers.data)
                        {
                            var wallpaper = new Models.Wallpaper.Entities.Wallpaper
                            {
                                UId = item.id,
                                Path = item.path,
                                Thumbnail = item.thumbs.large.PathToBitmapImage(),
                                DimensionX = item.dimension_x,
                                DimensionY = item.dimension_y,
                                WallpaperUrl = item.path,
                                WallpaperThumbUrl = item.thumbs.large,
                                ColorPalette = item.colors.Select(color => new Models.Wallpaper.Entities.Color
                                {
                                    ColorCode = color
                                }).ToObservableCollection()
                            };

                            wallpaper = db.Wallpapers
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
                            .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                            wallpapers.Add(wallpaper);
                        }

                        Wallpapers = wallpapers;
                        Pagination = new Pagination(jsonObjwallpapers.meta.total, CurrentPage, PageLimit, 15);
                    }
                    // load mobile wallpapers
                    else if (WallpapersType == WallpaperType.Mobile)
                    {
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<MobileWallpapersJson>(wallpapersData);

                        var wallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();
                        using var db = new AppDbContext();

                        foreach (var item in jsonObjwallpapers.items)
                        {
                            var wallpaper = new Models.Wallpaper.Entities.Wallpaper
                            {
                                UId = item.id.ToString(),
                                Path = item.variations.adapted.url,
                                Thumbnail = item.variations.preview_small.url.PathToBitmapImage(),
                                DimensionX = item.variations.adapted.resolution.width,
                                DimensionY = item.variations.adapted.resolution.height,
                                WallpaperUrl = item.variations.adapted.url,
                                WallpaperThumbUrl = item.variations.preview_small.url,
                                WallpaperType = WallpaperType.Mobile
                            };

                            wallpaper = db.Wallpapers
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
                            .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                            wallpapers.Add(wallpaper);
                        }

                        Wallpapers = wallpapers;
                        Pagination = new Pagination(jsonObjwallpapers.count, CurrentPage, PageLimit, 15);
                    }
                });
            }).Start();
        }

        /// <summary>
        /// Build API request query
        /// </summary>
        public void SetApiPath()
        {
            if (WallpapersType == WallpaperType.Desktop)
            {
                ApiUrl = $"https://wallhaven.cc/api/v1/search?page={CurrentPage}" +
                     $"&resolutions={SelectedResolution.ResolutionX}x{SelectedResolution.ResolutionY}";
            }
            else if (WallpapersType == WallpaperType.Mobile)
            {
                ApiUrl = $"https://api.wallpaperscraft.com/images?offset={CurrentPage * 30}" +
                 $"&screen[width]={SelectedResolution.ResolutionX}&screen[height]={SelectedResolution.ResolutionY}" +
                 $"&sort=date&types[]=free&types[]=private";
            }
        }

        /// <summary>
        /// Loads resolutions and ratios
        /// Sets last selected resolution and ratio
        /// </summary>
        public void LoadResolutionRatios()
        {
            using var db = new AppDbContext();

            var selectedRes = new Models.Wallpaper.Resolution();

            if (WallpapersType == WallpaperType.Desktop)
            {
                // get resolutions
                ResolutionRatios = GetDesktopResolutionRatios();

                // get last selected resolution
                var setting = db.AppSettings.FirstOrDefault(x => x.SettingName == "DesktopWallpaperResolution");
                var savedRes = setting.Value.Split('x');

                selectedRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = setting != null ? int.Parse(savedRes[0]) : 1920,
                    ResolutionY = setting != null ? int.Parse(savedRes[1]) : 1080,
                };
            }
            else if (WallpapersType == WallpaperType.Mobile)
            {
                // get resolutions
                ResolutionRatios = GetMobileResolutionRatios();

                // get last selected resolution
                var setting = db.AppSettings.FirstOrDefault(x => x.SettingName == "MobileWallpaperResolution");
                var savedRes = setting.Value.Split('x');

                selectedRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = setting != null ? int.Parse(savedRes[0]) : 1080,
                    ResolutionY = setting != null ? int.Parse(savedRes[1]) : 1920,
                };
            }

            // select resolution ratio
            SelectedResolutionRatio = ResolutionRatios?
                .FirstOrDefault(x => x.Resolutions
                    .Any(c => c.ResolutionX == selectedRes.ResolutionX && c.ResolutionY == selectedRes.ResolutionY));

            if (SelectedResolutionRatio == null || SelectedResolutionRatio.Resolutions.Count <= 0) return;

            foreach (var res in SelectedResolutionRatio.Resolutions)
            {
                if (!(res.ResolutionX == selectedRes.ResolutionX && res.ResolutionY == selectedRes.ResolutionY)) continue;

                // set selected resolution
                res.IsChecked = true;
                SelectedResolution = res;

                break;
            }
        }

        /// <summary>
        /// Apply set as commands
        /// </summary>
        /// <param name="wallpaper"></param>
        /// <param name="action"></param>
        public void ApplyAction(Models.Wallpaper.Entities.Wallpaper wallpaper, string action)
        {
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

                wallpaper.Id = 0;
                wallpaper.IsFavorite = false;
                wallpaper.Path = wallpaper.WallpaperUrl;
                wallpaper.Thumbnail = wallpaper.WallpaperThumbUrl.PathToBitmapImage();
            }
        }

        /// <summary>
        /// Extract color palette from image
        /// </summary>
        /// <param name="wallpaper"></param>
        public void ExtractColors(Models.Wallpaper.Entities.Wallpaper wallpaper)
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                // wallpaper image
                var image = new BitmapImage(new Uri(wallpaper.Path));

                // copy to byte array
                int stride = image.PixelWidth * 4;
                byte[] buffer = new byte[stride * image.PixelHeight];
                image.CopyPixels(buffer, stride, 0);

                // create bitmap
                System.Drawing.Bitmap bitmap =
                    new System.Drawing.Bitmap(
                        image.PixelWidth,
                        image.PixelHeight,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // lock bitmap data
                System.Drawing.Imaging.BitmapData bitmapData =
                    bitmap.LockBits(
                        new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height),
                        System.Drawing.Imaging.ImageLockMode.WriteOnly,
                        bitmap.PixelFormat);

                // copy byte array to bitmap data
                System.Runtime.InteropServices.Marshal.Copy(
                    buffer, 0, bitmapData.Scan0, buffer.Length);

                // unlock
                bitmap.UnlockBits(bitmapData);

                // extract colors
                var colorThief = new ColorThief();
                var palette = colorThief.GetPalette(bitmap, 5, 7);

                if (palette.Count > 0)
                {
                    foreach (var item in palette)
                    {
                        var color = new Models.Wallpaper.Entities.Color
                        {
                            WallpaperId = wallpaper.Id,
                            ColorCode = item.Color.ToHexString()
                        };

                        wallpaper.ColorPalette.Add(color);
                    }

                    if (wallpaper.ColorPalette.Count > 0)
                    {
                        using var db = new AppDbContext();
                        db.Colors.AddRange(wallpaper.ColorPalette);
                        db.SaveChanges();
                    }
                }
            }).Start();
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
        /// Download wallpaper and then set as ...
        /// </summary>
        /// <param name="parameter"></param>
        public void SetAs(object parameter)
        {
            var values = (object[])parameter;
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)values[0];
            SelectedWallpaper = wallpaper;

            if (wallpaper.Id <= 0)
            {
                ProgressVisibility = Visibility.Visible;

                var wallpaperPath = WallpapersType == WallpaperType.Desktop ? 
                $"{Settings.CurrentDirectory}\\Wallpapers\\{wallpaper.UId}{Path.GetExtension(wallpaper.Path)}"
                : $"{Settings.CurrentDirectory}\\Wallpapers\\Mobile\\{wallpaper.UId}{Path.GetExtension(wallpaper.Path)}";

                var thumbnailPath = $"{Settings.CurrentDirectory}\\Thumbnails\\" +
                $"{wallpaper.UId}{Path.GetExtension(wallpaper.Thumbnail.BitmapImageToPath())}";

                using var webClient = new WebClient();
                webClient.Headers.Add("user-agent", Settings.UserAgentHeader);
                webClient.DownloadProgressChanged += WebClient_DownloadProgress;

                webClient.DownloadFileCompleted += (sender, e) =>
                {
                    var webClient2 = new WebClient();
                    webClient2.DownloadProgressChanged += WebClient_DownloadProgress;
                    webClient2.DownloadFileCompleted += (sender, e) =>
                    {
                        ProgressVisibility = Visibility.Hidden;

                        wallpaper.Path = wallpaperPath;
                        wallpaper.Thumbnail = thumbnailPath.PathToBitmapImage();

                        if (wallpaper.ColorPalette?.Count <= 0)
                        {
                            ExtractColors(wallpaper);
                        }

                        using var db = new AppDbContext();
                        db.Wallpapers.Add(wallpaper);
                        db.SaveChanges();

                        ApplyAction(wallpaper, (string)values[1]);
                    };

                    webClient2.DownloadFileAsync(new Uri(wallpaper.Thumbnail.BitmapImageToPath()), thumbnailPath);
                };

                webClient.DownloadFileAsync(new Uri(wallpaper.Path), wallpaperPath);
            }
            else
            {
                ApplyAction(wallpaper, (string)values[1]);
            }
        }

        /// <summary>
        /// Get wallpapers with selected resolution
        /// </summary>
        /// <param name="sender"></param>
        public void ResolutionChanged(object sender)
        {
            // prevent reload to already loaded wallpapers
            if (ViewModelApplication.TempWallpapers != null) return;

            // get set selected resolution
            var resolution = (Models.Wallpaper.Resolution)sender;
            SelectedResolution = resolution;

            // save selected resolution
            using var db = new AppDbContext();
            var setting = new AppSetting();

            if (WallpapersType == WallpaperType.Desktop)
            {
                setting = db.AppSettings.First(x => x.SettingName == "DesktopWallpaperResolution");
            }
            else if (WallpapersType == WallpaperType.Mobile)
            {
                setting = db.AppSettings.First(x => x.SettingName == "MobileWallpaperResolution");
            }

            setting.Value = $"{resolution.ResolutionX}x{resolution.ResolutionY}";
            db.SaveChanges();


            foreach (var item in ResolutionRatios)
            {
                foreach (var i in item.Resolutions)
                {
                    if (i != resolution)
                    {
                        i.IsChecked = false;
                    }
                    else
                    {
                        SelectedResolutionRatio = item;
                    }
                }
            }

            CurrentPage = 1;
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

            if (WallpapersType == WallpaperType.Desktop)
            {
                ViewModelApplication.PreviousPage = ApplicationPage.DesktopWallpapers;
            }
            else if (WallpapersType == WallpaperType.Mobile)
            {
                ViewModelApplication.PreviousPage = ApplicationPage.MobileWallpapers;
            }

            ViewModelApplication.GoToPage(ApplicationPage.Wallpaper);
        }

        /// <summary>
        /// Add to favorites
        /// </summary>
        /// <param name="sender"></param>
        public void SetFavoriteWallpaper(object sender)
        {
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)sender;

            if (wallpaper.Path.StartsWith("https://"))
            {
                SetAs(new object[] { wallpaper, "" });
            }

            using var db = new AppDbContext();
            db.Wallpapers.Update(wallpaper);
            db.SaveChanges();
        }

        #endregion


        #region Events

        private void WebClient_DownloadProgress(object sender, DownloadProgressChangedEventArgs e)
        {
            DownloadProgress = e.ProgressPercentage;
        }

        #endregion
    }
}