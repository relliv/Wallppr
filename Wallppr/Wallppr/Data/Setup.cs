using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wallppr.Data
{
    public class Setup
    {
        public Setup()
        {
            ResourceDirectories = new List<string>
            {
                @$"{Settings.CurrentDirectory}\Wallpapers",
                @$"{Settings.CurrentDirectory}\Wallpapers\Mobile",
                @$"{Settings.CurrentDirectory}\Thumbnails"
            };

            CheckResourcesFolder();
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

    }
}