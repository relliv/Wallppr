using Wallppr.Models.Wallpaper.Enums;

namespace Wallppr.ViewModel.Wallpaper
{
    public class DesktopWallpapersViewModel : WallpapersCommonViewModel
    {
        public DesktopWallpapersViewModel()
        {
            WallpapersType = WallpaperType.Desktop;

            LoadResolutionRatios();
            LoadWallpapers();
        }
    }
}