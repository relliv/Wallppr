using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;
using Wallppr.Models.Wallpaper.Enums;

namespace Wallppr.Models.Wallpaper.Entities
{
    public class Wallpaper : INotifyPropertyChanged
    {
        public Wallpaper()
        {
            AddedDate = DateTime.Now;

            ColorPalette = new ObservableCollection<Color>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        public string UId { get; set; }
        public DateTime AddedDate { get; set; }
        public string Path { get; set; }
        public BitmapImage Thumbnail { get; set; }
        public string WallpaperUrl { get; set; }
        public string WallpaperThumbUrl { get; set; }
        public int DimensionX { get; set; }
        public int DimensionY { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsDownloaded { get; set; }
        public WallpaperType WallpaperType { get; set; }

        public virtual ObservableCollection<Color> ColorPalette { get; set; }
        public virtual ObservableCollection<History> History { get; set; }
    }
}
