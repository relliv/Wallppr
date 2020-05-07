using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Wallppr.Models.AppSetting
{
    public class AppSetting : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public long Id { get; set; }
        [Required]
        public string SettingName { get; set; }
        public string Value { get; set; }
        public bool IsEditable { get; set; }
        public string DefaultValue { get; set; }
    }
}