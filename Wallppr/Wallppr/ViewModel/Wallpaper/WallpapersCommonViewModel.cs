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
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Wallppr.Data;
using Wallppr.Helpers;
using Wallppr.Models.AppSetting;
using Wallppr.Models.Common;
using Wallppr.Models.Wallpaper;
using Wallppr.Models.Wallpaper.Entities;
using Wallppr.Models.Wallpaper.Enums;
using Wallppr.Models.Wallpaper.Json;
using Wallppr.UI.i18N;
using static Wallppr.DI.DI;
using static Wallppr.Helpers.GetResolutions;

namespace Wallppr.ViewModel.Wallpaper
{
    public class WallpapersCommonViewModel : ViewModelBase
    {
        public WallpapersCommonViewModel()
        {
            WallhavenApiUrl = "https://wallhaven.cc/api/v1/search";
            WallpapersCraftApiUrl = "https://api.wallpaperscraft.com/images";

            // go to page commands
            GotoPageRandomCommand = new RelayParameterizedCommand(GoToPageRandom);
            GoToPageLatestCommand = new RelayParameterizedCommand(GoToPageLatest);

            TabSelectionChangedCommand = new RelayParameterizedCommand(TabSelectionChanged);
            ResolutionChangedCommand = new RelayParameterizedCommand(ResolutionChanged);
            ShowWallpaperCommand = new RelayParameterizedCommand(ShowWallpaper);
            SetAsCommand = new RelayParameterizedCommand(SetAs);
            SetFavoriteWallpaperCommand = new RelayParameterizedCommand(SetFavoriteWallpaper);

            RandomWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();
            LatestWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();

            ProgressVisibility = Visibility.Hidden;
        }

        #region Commands

        public ICommand SetAsCommand { get; set; }
        public ICommand SetFavoriteWallpaperCommand { get; set; }
        public ICommand ResolutionChangedCommand { get; set; }
        public ICommand ShowWallpaperCommand { get; set; }
        public ICommand TabSelectionChangedCommand { get; set; }

        #region Goto Page

        public ICommand GotoPageRandomCommand { get; set; }
        public ICommand GoToPageLatestCommand { get; set; }

        #endregion

        #endregion


        #region Public Properties

        #region API

        public string WallhavenApiUrl { get; set; }
        public string WallpapersCraftApiUrl { get; set; }

        #endregion

        #region Wallpaper

        #region Random Wallpaper

        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> RandomWallpapers { get; set; }
        public Visibility RandomWallpapersLIVisibility { get; set; }
        public Models.Wallpaper.Entities.Wallpaper SelectedRandomWallpaper { get; set; }
        public bool IsProcessingRandomWallpapers { get; set; }

        #endregion

        #region Latest Wallpaper

        public ObservableCollection<Models.Wallpaper.Entities.Wallpaper> LatestWallpapers { get; set; }
        public Visibility LatestWallpapersLIVisibility { get; set; }
        public Models.Wallpaper.Entities.Wallpaper SelectedLatestWallpaper { get; set; }
        public bool IsProcessingLatestWallpapers { get; set; }

        #endregion

        public Models.Wallpaper.Entities.Wallpaper SelectedWallpaper { get; set; }
        public WallpaperType WallpapersType { get; set; }
        public double DownloadProgress { get; set; }
        public Visibility ProgressVisibility { get; set; }

        #endregion

        #region Resolution

        #region Random

        public List<ResolutionRatio> RandomResolutionRatios { get; set; }
        public ResolutionRatio SelectedRandomResolutionRatio { get; set; }
        public Models.Wallpaper.Resolution SelectedRandomResolution { get; set; }

        #endregion

        #region Latest

        public List<ResolutionRatio> LatestResolutionRatios { get; set; }
        public ResolutionRatio SelectedLatestResolutionRatio { get; set; }
        public Models.Wallpaper.Resolution SelectedLatestResolution { get; set; }

        #endregion

        #endregion

        #endregion


        #region Pagination

        #region Random

        public Pagination RandomPagination { get; set; }
        public int RandomPageLimit { get; set; } = 24;
        public int RandomCurrentPage { get; set; } = 1;
        public string RandomSearchTerm { get; set; }

        #endregion

        #region Latest

        public Pagination LatestPagination { get; set; }
        public int LatestPageLimit { get; set; } = 24;
        public int LatestCurrentPage { get; set; } = 1;
        public string LatestSearchTerm { get; set; }

        #endregion

        #endregion


        #region Methods

        /// <summary>
        /// Fetch and load random wallpapers from apis
        /// </summary>
        public void LoadRandomWallpapers()
        {
            if (IsProcessingRandomWallpapers)
                return;

            new Task(async () =>
            {
                IsProcessingRandomWallpapers = true;
                RandomWallpapersLIVisibility = Visibility.Visible;

                await Task.Delay(700);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    // set empty wallpaper list
                    RandomWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();

                    // load desktop wallpapers
                    if (WallpapersType == WallpaperType.Desktop)
                    {
                        // random wallpapers api url
                        var apiUrl = $"{WallhavenApiUrl}?page={RandomCurrentPage}&sorting=random" +
                        $"&resolutions={SelectedRandomResolution.ResolutionX}x{SelectedRandomResolution.ResolutionY}";

                        // get data
                        var wallpapersData = apiUrl.Get();

                        // if has not valid server response data
                        if (wallpapersData == null)
                            return;

                        // parsed wallpapers data
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<WallpapersJson>(wallpapersData);

                        if (jsonObjwallpapers != null)
                        {
                            using var db = new AppDbContext();

                            // loop in wallpaper data
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
                                    ColorPalette = item.colors?.Select(color => new Models.Wallpaper.Entities.Color
                                    {
                                        ColorCode = color
                                    }).ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id).ToObservableCollection()
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
                                    .ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id).ToObservableCollection()
                                })
                                .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                                RandomWallpapers.Add(wallpaper);
                            }

                            // select first wallpaper and scroll to top
                            SelectedRandomWallpaper = RandomWallpapers.FirstOrDefault();

                            // wallpaper pagination
                            RandomPagination = new Pagination(jsonObjwallpapers.meta.total, RandomCurrentPage, RandomPageLimit, 10);
                        }
                    }
                    // load mobile wallpapers
                    else if (WallpapersType == WallpaperType.Mobile)
                    {
                        // random wallpapers api url
                        var apiUrl = $"{WallpapersCraftApiUrl}/shuffle?offset={RandomCurrentPage * 30}" +
                        $"&screen[width]={SelectedRandomResolution.ResolutionX}&screen[height]={SelectedRandomResolution.ResolutionY}" +
                        $"&sort=date&types[]=free&types[]=public";

                        // get data
                        var wallpapersData = apiUrl.Get();

                        // if has not valid server response data
                        if (wallpapersData == null)
                            return;

                        // parsed wallpapers data
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<MobileWallpapersJson>(wallpapersData);

                        if (jsonObjwallpapers != null)
                        {
                            using var db = new AppDbContext();

                            // loop in wallpapers data
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
                                    WallpaperType = WallpaperType.Mobile,
                                    History = db.History.Where(x => x.WallpaperUId == item.id.ToString()).ToObservableCollection()
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
                                    .ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id.ToString()).ToObservableCollection()
                                })
                                .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                                RandomWallpapers.Add(wallpaper);
                            }

                            // select first wallpaper and scroll to top
                            SelectedRandomWallpaper = RandomWallpapers.FirstOrDefault();

                            // wallpaper pagination
                            RandomPagination = new Pagination(jsonObjwallpapers.count, RandomCurrentPage, RandomPageLimit, 10);
                        }
                    }
                });

                RandomWallpapersLIVisibility = Visibility.Hidden;
                IsProcessingRandomWallpapers = false;
            }).Start();
        }

        /// <summary>
        /// Fetch and load latest wallpapers from apis
        /// </summary>
        public void LoadLatestWallpapers()
        {
            if (IsProcessingLatestWallpapers)
                return;

            new Task(async () =>
            {
                IsProcessingLatestWallpapers = true;
                LatestWallpapersLIVisibility = Visibility.Visible;

                await Task.Delay(1000);

                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    // set empty wallpaper list
                    LatestWallpapers = new ObservableCollection<Models.Wallpaper.Entities.Wallpaper>();

                    // load desktop wallpapers
                    if (WallpapersType == WallpaperType.Desktop)
                    {
                        // random wallpapers api url
                        var apiUrl = $"{WallhavenApiUrl}?page={LatestCurrentPage}" +
                        $"&resolutions={SelectedLatestResolution.ResolutionX}x{SelectedLatestResolution.ResolutionY}";

                        // get data
                        var wallpapersData = apiUrl.Get();

                        // if has not valid server response data
                        if (wallpapersData == null)
                            return;

                        // parsed wallpapers data
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<WallpapersJson>(wallpapersData);

                        if (jsonObjwallpapers != null)
                        {
                            using var db = new AppDbContext();

                            // loop in wallpapers data
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
                                    ColorPalette = item.colors?.Select(color => new Models.Wallpaper.Entities.Color
                                    {
                                        ColorCode = color
                                    }).ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id).ToObservableCollection()
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
                                    .ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id).ToObservableCollection()
                                })
                                .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                                LatestWallpapers.Add(wallpaper);
                            }

                            // select first wallpaper and scroll to top
                            SelectedLatestWallpaper = LatestWallpapers.FirstOrDefault();

                            // wallpaper pagination
                            LatestPagination = new Pagination(jsonObjwallpapers.meta.total, LatestCurrentPage, LatestPageLimit, 10);
                        }
                    }
                    // load mobile wallpapers
                    else if (WallpapersType == WallpaperType.Mobile)
                    {
                        // random wallpapers api url
                        var apiUrl = $"{WallpapersCraftApiUrl}?offset={LatestCurrentPage * 30}" +
                        $"&screen[width]={SelectedLatestResolution.ResolutionX}&screen[height]={SelectedLatestResolution.ResolutionY}" +
                        $"&sort=date&types[]=free&types[]=public";

                        // get data
                        var wallpapersData = apiUrl.Get();

                        // if has not valid server response data
                        if (wallpapersData == null)
                            return;

                        // parsed wallpapers data
                        var jsonObjwallpapers = JsonConvert.DeserializeObject<MobileWallpapersJson>(wallpapersData);

                        if (jsonObjwallpapers != null)
                        {
                            using var db = new AppDbContext();

                            // loop in wallpapers data
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
                                    WallpaperType = WallpaperType.Mobile,
                                    History = db.History.Where(x => x.WallpaperUId == item.id.ToString()).ToObservableCollection()
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
                                    .ToObservableCollection(),
                                    History = db.History.Where(x => x.WallpaperUId == item.id.ToString()).ToObservableCollection()
                                })
                                .FirstOrDefault(x => x.UId == wallpaper.UId) ?? wallpaper;

                                LatestWallpapers.Add(wallpaper);
                            }

                            // select first wallpaper and scroll to top
                            SelectedLatestWallpaper = LatestWallpapers.FirstOrDefault();

                            // wallpaper pagination
                            LatestPagination = new Pagination(jsonObjwallpapers.count, LatestCurrentPage, LatestPageLimit, 10);
                        }
                    }
                });

                LatestWallpapersLIVisibility = Visibility.Hidden;
                IsProcessingLatestWallpapers = false;
            }).Start();
        }

        /// <summary>
        /// Loads resolutions and ratios
        /// Sets last selected resolution and ratio
        /// </summary>
        public void LoadResolutionRatios()
        {
            using var db = new AppDbContext();

            var selectedRandomRes = new Models.Wallpaper.Resolution();
            var selectedLatestRes = new Models.Wallpaper.Resolution();

            if (WallpapersType == WallpaperType.Desktop)
            {
                var resolutions = GetDesktopResolutionRatios();
                RandomResolutionRatios = resolutions;
                LatestResolutionRatios = resolutions;

                #region Random Resoltuion

                // get last random selected resolution
                var randomResolutionsetting = db.AppSettings.FirstOrDefault(x => x.SettingName == "DesktopRandomWallpaperResolution");
                var savedRandomRes = randomResolutionsetting.Value.Split('x');

                selectedRandomRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = randomResolutionsetting != null ? int.Parse(savedRandomRes[0]) : 1920,
                    ResolutionY = randomResolutionsetting != null ? int.Parse(savedRandomRes[1]) : 1080,
                };

                #endregion

                #region Latest Resolution

                // get last latest selected resolution
                var latestResolutionsetting = db.AppSettings.FirstOrDefault(x => x.SettingName == "DesktopLastestWallpaperResolution");
                var savedLatestRes = latestResolutionsetting.Value.Split('x');

                selectedLatestRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = latestResolutionsetting != null ? int.Parse(savedLatestRes[0]) : 1920,
                    ResolutionY = latestResolutionsetting != null ? int.Parse(savedLatestRes[1]) : 1080,
                };

                #endregion
            }
            else if (WallpapersType == WallpaperType.Mobile)
            {
                var resolutions = GetMobileResolutionRatios();
                RandomResolutionRatios = resolutions;
                LatestResolutionRatios = resolutions;

                #region Random Resoltuion

                // get last random selected resolution
                var randomResolutionsetting = db.AppSettings.FirstOrDefault(x => x.SettingName == "MobileRandomWallpaperResolution");
                var savedRandomRes = randomResolutionsetting.Value.Split('x');

                selectedRandomRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = randomResolutionsetting != null ? int.Parse(savedRandomRes[0]) : 1080,
                    ResolutionY = randomResolutionsetting != null ? int.Parse(savedRandomRes[1]) : 1920,
                };

                #endregion

                #region Latest Resolution

                // get last latest selected resolution
                var latestResolutionsetting = db.AppSettings.FirstOrDefault(x => x.SettingName == "MobileLatestWallpaperResolution");
                var savedLatestRes = latestResolutionsetting.Value.Split('x');

                selectedLatestRes = new Models.Wallpaper.Resolution
                {
                    ResolutionX = latestResolutionsetting != null ? int.Parse(savedLatestRes[0]) : 1920,
                    ResolutionY = latestResolutionsetting != null ? int.Parse(savedLatestRes[1]) : 1080,
                };

                #endregion
            }

            #region Random Resoltuion

            // select resolution ratio
            SelectedRandomResolutionRatio = RandomResolutionRatios?
            .FirstOrDefault(x => x.Resolutions
            .Any(c => c.ResolutionX == selectedRandomRes.ResolutionX && c.ResolutionY == selectedRandomRes.ResolutionY));

            if (SelectedRandomResolutionRatio == null || SelectedRandomResolutionRatio.Resolutions.Count <= 0) return;

            foreach (var res in SelectedRandomResolutionRatio.Resolutions)
            {
                if (!(res.ResolutionX == selectedRandomRes.ResolutionX && res.ResolutionY == selectedRandomRes.ResolutionY)) continue;

                // set selected resolution
                res.IsChecked = true;
                SelectedRandomResolution = res;

                break;
            }

            #endregion

            #region Latest Resolution

            // select resolution ratio
            SelectedLatestResolutionRatio = LatestResolutionRatios?
            .FirstOrDefault(x => x.Resolutions
            .Any(c => c.ResolutionX == selectedLatestRes.ResolutionX && c.ResolutionY == selectedLatestRes.ResolutionY));

            if (SelectedLatestResolutionRatio == null || SelectedLatestResolutionRatio.Resolutions.Count <= 0) return;

            foreach (var res in SelectedLatestResolutionRatio.Resolutions)
            {
                if (!(res.ResolutionX == selectedLatestRes.ResolutionX && res.ResolutionY == selectedLatestRes.ResolutionY)) continue;

                // set selected resolution
                res.IsChecked = true;
                SelectedLatestResolution = res;

                break;
            }

            #endregion
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
                    Description = ViewModelApplication.LanguageResourceDictionary["SelectSaveDestination"].ToString(),
                    RootFolder = Environment.SpecialFolder.Desktop
                };

                var result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrEmpty(folderBrowserDialog.SelectedPath))
                {
                    var fileName = Path.GetFileName(wallpaper.Path);

                    File.Copy(wallpaper.Path, Path.Combine(folderBrowserDialog.SelectedPath, fileName));
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
        /// Tab selection changed
        /// </summary>
        /// <param name="sender"></param>
        public void TabSelectionChanged(object sender)
        {
            var tabIndex = (int)sender;

            if (tabIndex == 1 && (LatestWallpapers == null || LatestWallpapers.Count <= 0))
            {
                LoadLatestWallpapers();
            }
        }

        /// <summary>
        /// Go to selected random page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPageRandom(object sender)
        {
            var page = (Models.Common.Page)sender;

            RandomCurrentPage = page.PageNumber;
            LoadRandomWallpapers();
        }

        /// <summary>
        /// Go to selected latest page
        /// </summary>
        /// <param name="sender"></param>
        public void GoToPageLatest(object sender)
        {
            var page = (Models.Common.Page)sender;

            LatestCurrentPage = page.PageNumber;
            LoadLatestWallpapers();
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
                $@"{Settings.WallpaperDesktopFolder}\{wallpaper.UId}{Path.GetExtension(wallpaper.Path)}"
                : $@"{Settings.WallpaperMobileFolder}\\{wallpaper.UId}{Path.GetExtension(wallpaper.Path)}";

                var thumbnailPath = $@"{Settings.WallpaperThumbnailsFolder}\" +
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

                        // has not extracted colors
                        if (wallpaper.ColorPalette?.Count <= 0)
                        {
                            ExtractColors(wallpaper);
                        }

                        wallpaper.IsDownloaded = true;

                        using var db = new AppDbContext();

                        // if wallpaper is not saved
                        if (wallpaper.Id == 0)
                        {
                            db.Wallpapers.Add(wallpaper);
                            db.SaveChanges();
                        }

                        // check history for today
                        var todayHistory = db.History.Any(x => x.WallpaperId == wallpaper.Id && x.AddedDate.Date != DateTime.Now.Date);

                        if (!todayHistory)
                        {
                            var history = new History
                            {
                                WallpaperId = wallpaper.Id,
                                WallpaperUId = SelectedWallpaper.UId
                            };

                            // add to history
                            db.History.Add(history);
                            db.SaveChanges();

                            wallpaper.History.Add(history);
                        }

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
        public void ResolutionChanged(object parameters)
        {
            var values = (object[])parameters;

            // get set selected resolution
            var resolution = (Models.Wallpaper.Resolution)values[0];

            // resolution type
            var type = (string)values[1];

            using var db = new AppDbContext();

            if (type == "Random")
            {
                SelectedRandomResolution = resolution;

                var setting = new AppSetting();

                if (WallpapersType == WallpaperType.Desktop)
                {
                    setting = db.AppSettings.First(x => x.SettingName == "DesktopRandomWallpaperResolution");
                }
                else if (WallpapersType == WallpaperType.Mobile)
                {
                    setting = db.AppSettings.First(x => x.SettingName == "MobileRandomWallpaperResolution");
                }

                setting.Value = $"{resolution.ResolutionX}x{resolution.ResolutionY}";
                db.SaveChanges();

                foreach (var item in RandomResolutionRatios)
                {
                    foreach (var i in item.Resolutions)
                    {
                        if (i != resolution)
                        {
                            i.IsChecked = false;
                        }
                        else
                        {
                            SelectedRandomResolutionRatio = item;
                        }
                    }
                }

                RandomCurrentPage = 1;
                LoadRandomWallpapers();
            }
            else if (type == "Latest")
            {
                SelectedLatestResolution = resolution;

                var setting = new AppSetting();

                if (WallpapersType == WallpaperType.Desktop)
                {
                    setting = db.AppSettings.First(x => x.SettingName == "DesktopLastestWallpaperResolution");
                }
                else if (WallpapersType == WallpaperType.Mobile)
                {
                    setting = db.AppSettings.First(x => x.SettingName == "MobileLatestWallpaperResolution");
                }

                setting.Value = $"{resolution.ResolutionX}x{resolution.ResolutionY}";
                db.SaveChanges();

                foreach (var item in LatestResolutionRatios)
                {
                    foreach (var i in item.Resolutions)
                    {
                        if (i != resolution)
                        {
                            i.IsChecked = false;
                        }
                        else
                        {
                            SelectedLatestResolutionRatio = item;
                        }
                    }
                }

                RandomCurrentPage = 1;
                LoadRandomWallpapers();
            }
        }

        /// <summary>
        /// Show wallpaper with orginal size
        /// </summary>
        /// <param name="sender"></param>
        public void ShowWallpaper(object sender)
        {
            var wallpaper = (Models.Wallpaper.Entities.Wallpaper)sender;

            ViewModelApplication.SelectedWallpaper = wallpaper;
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