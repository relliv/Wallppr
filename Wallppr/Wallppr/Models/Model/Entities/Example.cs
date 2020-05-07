using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media.Imaging;

namespace Wallppr.Models.Model
{
    public class Example : INotifyPropertyChanged
    {
        public Example()
        {
            AddedDate = DateTime.Now;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public DateTime AddedDate { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public double Width { get; set; }
        public BitmapImage Image { get; set; }
        public TimeSpan SpendTime { get; set; }
    }
}
