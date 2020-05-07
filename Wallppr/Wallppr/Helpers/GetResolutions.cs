using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Wallppr.Models.Wallpaper;

namespace Wallppr.Helpers
{
    public static class GetResolutions
    {
        public static List<ResolutionRatio> GetDesktopResolutionRatios()
        {
            return new List<ResolutionRatio>
            {
                // Ratio 16:9
                new ResolutionRatio
                {
                    Ratio = "16:9",
                    Resolutions = new ObservableCollection<Resolution>
                    {
                        new Resolution
                        {
                            ResolutionX = 1280,
                            ResolutionY = 720
                        },
                        new Resolution
                        {
                            ResolutionX = 1600,
                            ResolutionY = 900
                        },
                        new Resolution
                        {
                            ResolutionX = 1920,
                            ResolutionY = 1080
                        },
                        new Resolution
                        {
                            ResolutionX = 2560,
                            ResolutionY = 1440
                        },
                        new Resolution
                        {
                            ResolutionX = 3840,
                            ResolutionY = 2160
                        }
                    }
                },
                // Ratio 16:10
                new ResolutionRatio
                {
                    Ratio = "16:10",
                    Resolutions = new ObservableCollection<Resolution>
                    {
                        new Resolution
                        {
                            ResolutionX = 1280,
                            ResolutionY = 800
                        },
                        new Resolution
                        {
                            ResolutionX = 1600,
                            ResolutionY = 1000
                        },
                        new Resolution
                        {
                            ResolutionX = 1920,
                            ResolutionY = 1200
                        },
                        new Resolution
                        {
                            ResolutionX = 2560,
                            ResolutionY = 1600
                        },
                        new Resolution
                        {
                            ResolutionX = 3840,
                            ResolutionY = 2400
                        }
                    }
                },
                // Ratio 4:3
                new ResolutionRatio
                {
                    Ratio = "4:3",
                    Resolutions = new ObservableCollection<Resolution>
                    {
                        new Resolution
                        {
                            ResolutionX = 1280,
                            ResolutionY = 960
                        },
                        new Resolution
                        {
                            ResolutionX = 1600,
                            ResolutionY = 1200
                        },
                        new Resolution
                        {
                            ResolutionX = 1920,
                            ResolutionY = 1440
                        },
                        new Resolution
                        {
                            ResolutionX = 2560,
                            ResolutionY = 1920
                        },
                        new Resolution
                        {
                            ResolutionX = 3840,
                            ResolutionY = 2880
                        }
                    }
                },
                // Ratio 5:4
                new ResolutionRatio
                {
                    Ratio = "5:4",
                    Resolutions = new ObservableCollection<Resolution>
                    {
                        new Resolution
                        {
                            ResolutionX = 1280,
                            ResolutionY = 1024
                        },
                        new Resolution
                        {
                            ResolutionX = 1600,
                            ResolutionY = 1280
                        },
                        new Resolution
                        {
                            ResolutionX = 1920,
                            ResolutionY = 1536
                        },
                        new Resolution
                        {
                            ResolutionX = 2560,
                            ResolutionY = 2048
                        },
                        new Resolution
                        {
                            ResolutionX = 3840,
                            ResolutionY = 3072
                        }
                    }
                },
                // Ratio Ultrawide
                new ResolutionRatio
                {
                    Ratio = "Ultrawide",
                    Resolutions =  new ObservableCollection<Resolution>
                    {
                        new Resolution
                        {
                            ResolutionX = 2560,
                            ResolutionY = 1080
                        },
                        new Resolution
                        {
                            ResolutionX = 3440,
                            ResolutionY = 1440
                        },
                        new Resolution
                        {
                            ResolutionX = 3840,
                            ResolutionY = 1600
                        }
                    }
                }
            };
        }

        public static List<ResolutionRatio> GetMobileResolutionRatios()
        {
            var desktopResolutionRatios = GetDesktopResolutionRatios();

            foreach(var ratio in desktopResolutionRatios)
            {
                if (ratio.Ratio.Contains(":"))
                {
                    var newRatio = ratio.Ratio.Split(':');
                    ratio.Ratio = $"{newRatio[1]}:{newRatio[0]}";
                }

                foreach (var resolution in ratio.Resolutions)
                {
                    var res = new Resolution
                    {
                        ResolutionX =  resolution.ResolutionX,
                        ResolutionY = resolution.ResolutionY
                    };

                    resolution.ResolutionX = res.ResolutionY;
                    resolution.ResolutionY = res.ResolutionX;
                }
            }

            return desktopResolutionRatios;
        }
    }
}