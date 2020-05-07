using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Wallppr.Models.Wallpaper
{
    public class ResolutionRatio : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Ratio { get; set; }

        public ObservableCollection<Resolution> Resolutions { get; set; }
    }
}