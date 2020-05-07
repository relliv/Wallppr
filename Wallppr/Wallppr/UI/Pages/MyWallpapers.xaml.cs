using Wallppr.ViewModel.Wallpaper;

namespace Wallppr.UI.Pages
{
    /// <summary>
    /// Interaction logic for MyWallpapers.xaml
    /// </summary>
    public partial class MyWallpapers : BasePage<MyWallpapersViewModel>
    {
        public MyWallpapers() : base()
        {
            InitializeComponent();
        }

        public MyWallpapers(MyWallpapersViewModel vm) : base(vm)
        {
            InitializeComponent();
        }
    }
}