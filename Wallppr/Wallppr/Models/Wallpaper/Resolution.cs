using System.ComponentModel;

namespace Wallppr.Models.Wallpaper
{
    public class Resolution : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public int ResolutionX { get; set; }
        public int ResolutionY { get; set; }
        public bool IsChecked { get; set; }
    }
}