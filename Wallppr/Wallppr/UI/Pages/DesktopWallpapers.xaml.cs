using Wallppr.ViewModel.Wallpaper;

namespace Wallppr.UI.Pages
{
    /// <summary>
    /// Interaction logic for DesktopWallpapers.xaml
    /// </summary>
    public partial class DesktopWallpapers : BasePage<DesktopWallpapersViewModel>
    {
        public DesktopWallpapers() : base()
        {
            InitializeComponent();
        }

        public DesktopWallpapers(DesktopWallpapersViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}