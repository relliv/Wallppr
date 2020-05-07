using Wallppr.ViewModel.Wallpaper;

namespace Wallppr.UI.Pages
{
    /// <summary>
    /// Interaction logic for Wallpaper.xaml
    /// </summary>
    public partial class Wallpaper : BasePage<WallpaperViewModel>
    {
        public Wallpaper() : base()
        {
            InitializeComponent();
        }

        public Wallpaper(WallpaperViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}