using Wallppr.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallppr.Models.Common
{
    public class AppSettings
    {
        public AppSettings()
        {
            LoadSettings();
        }

        public void LoadSettings()
        {
            using var db = new AppDbContext();

            var settings = db.AppSettings.ToList();

            FullName = settings.FirstOrDefault(x => x.SettingName == "FullName")?.Value;

            OpenWeatherApiKey = settings.FirstOrDefault(x => x.SettingName == "OpenWeatherApiKey")?.Value;
            OpenWeatherCity = settings.FirstOrDefault(x => x.SettingName == "OpenWeatherCity")?.Value ?? "Istanbul";
            OpenWeatherCountry = settings.FirstOrDefault(x => x.SettingName == "OpenWeatherCountry")?.Value ?? "TR";
        }

        public string FullName { get; set; }

        public string OpenWeatherApiKey { get; set; }
        public string OpenWeatherCity { get; set; }
        public string OpenWeatherCountry { get; set; }
    }
}