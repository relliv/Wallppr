using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Wallppr.DI.DI;

namespace Wallppr.ViewModel.Wallpaper
{
    public class WallpaperViewModel : WallpapersCommonViewModel
    {
        public WallpaperViewModel()
        {
            SelectedWallpaper = ViewModelApplication.SelectedWallpaper;
        }

        #region Property


        #endregion
    }
}