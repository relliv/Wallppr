using System.ComponentModel;

namespace Wallppr.Models.Wallpaper.Entities
{
    public class Color : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        public long WallpaperId { get; set; }
        public string ColorCode { get; set; }
    }
}