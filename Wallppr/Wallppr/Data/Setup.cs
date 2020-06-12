using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Wallppr.UI.i18N;
using Wallppr.Helpers;
using KOR.ReleaseCheck;

namespace Wallppr.Data
{
    public class Setup
    {
        public Setup()
        {
            var userAppFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            ResourceDirectories = new List<string>
            {
                Settings.WallpaperDesktopFolder,
                Settings.WallpaperMobileFolder,
                Settings.WallpaperThumbnailsFolder
            };

            CheckResourcesFolder();
            ConfitureApp();
            SetupLanguage();
        }

        public List<string> ResourceDirectories { get; set; }

        public void CheckResourcesFolder()
        {
            if (ResourceDirectories.Count > 0)
            {
                foreach (var dir in ResourceDirectories)
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                }
            }
        }

        public void ConfitureApp()
        {
            using var db = new AppDbContext();

            var isConfiguredSetting = db.AppSettings.Where(x => x.SettingName == "IsAppConfigured").FirstOrDefault();

            if (isConfiguredSetting.Value == "0")
            {
                var availableLangs = i18N.GetAvailableLangauges();
                var currentLang = Thread.CurrentThread.CurrentCulture.Name;

                if (availableLangs.Contains(currentLang))
                {
                    i18N.SwitchCurrentLanguage(currentLang);
                }

                isConfiguredSetting.Value = "1";
                db.SaveChanges();
            }
        }

        public void SetupLanguage()
        {
            i18N.LoadCurrentLanguage();
        }

    }
}