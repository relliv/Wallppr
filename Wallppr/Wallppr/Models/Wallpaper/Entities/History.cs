using System;
using System.ComponentModel;

namespace Wallppr.Models.Wallpaper.Entities
{
    public class History : INotifyPropertyChanged
    {
        public History()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        public long WallpaperId { get; set; }
        public string WallpaperUId { get; set; }
        public DateTime AddedDate { get; set; }
    }
}