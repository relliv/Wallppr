using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace Wallppr.Models.Wallpaper
{
    public class WallpaperItem
    {
        public Models.Wallpaper.Entities.Wallpaper Wallpaper { get; set; }

        public double DownloadProgress { get; set; }
    }
}
