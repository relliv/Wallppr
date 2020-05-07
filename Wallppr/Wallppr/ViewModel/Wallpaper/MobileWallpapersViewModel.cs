using Wallppr.Models.Wallpaper.Enums;

namespace Wallppr.ViewModel.Wallpaper
{
    public class MobileWallpapersViewModel : WallpapersCommonViewModel
    {
        public MobileWallpapersViewModel()
        {
            WallpapersType = WallpaperType.Mobile;

            LoadResolutionRatios();
            LoadWallpapers();
        }
    }
}