using Wallppr.ViewModel.Wallpaper;

namespace Wallppr.UI.Pages
{
    /// <summary>
    /// Interaction logic for MobileWallpapers.xaml
    /// </summary>
    public partial class MobileWallpapers : BasePage<MobileWallpapersViewModel>
    {
        public MobileWallpapers() : base()
        {
            InitializeComponent();
        }

        public MobileWallpapers(MobileWallpapersViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}