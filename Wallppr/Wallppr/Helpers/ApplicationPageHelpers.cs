using Wallppr.Models.Common;
using Wallppr.UI.Pages;
using Wallppr.ViewModel.App;
using Wallppr.ViewModel.Wallpaper;

namespace Wallppr.Helpers
{
    /// <summary>
    /// Converts the <see cref="ApplicationPage"/> to an actual view/page
    /// </summary>
    public static class ApplicationPageHelpers
    {
        /// <summary>
        /// Takes a <see cref="ApplicationPage"/> and a view model, if any, and creates the desired page
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public static BasePage ToBasePage(this ApplicationPage page, object viewModel = null)
        {
            // Find the appropriate page
            switch (page)
            {
                case ApplicationPage.WelcomePage:
                    return new WelcomePage(viewModel as WelcomeViewModel);
                case ApplicationPage.AppSettings:
                    return new UI.Pages.AppSettings();
                case ApplicationPage.DesktopWallpapers:
                    return new DesktopWallpapers();
                case ApplicationPage.MobileWallpapers:
                    return new MobileWallpapers();
                case ApplicationPage.MyWallpapers:
                    return new MyWallpapers(viewModel as MyWallpapersViewModel);
                case ApplicationPage.Wallpaper:
                    return new Wallpaper(viewModel as WallpaperViewModel);
                default:
                    // Debugger.Break();
                    return null;
            }
        }

        /// <summary>
        /// Converts a <see cref="BasePage"/> to the specific <see cref="ApplicationPage"/> that is for that type of page
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static ApplicationPage ToApplicationPage(this BasePage page)
        {
            // Alert developer of issue
            //Debugger.Break();
            return default(ApplicationPage);
        }
    }
}
