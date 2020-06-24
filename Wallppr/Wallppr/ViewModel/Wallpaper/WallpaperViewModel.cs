using System;
using System.Linq;
using Wallppr.Models.Wallpaper.Entities;
using Wallppr.Data;
using static Wallppr.DI.DI;

namespace Wallppr.ViewModel.Wallpaper
{
    public class WallpaperViewModel : WallpapersCommonViewModel
    {
        public WallpaperViewModel()
        {
            SelectedWallpaper = ViewModelApplication.SelectedWallpaper;

            SaveHistory();
        }

        #region Property


        #endregion

        #region Methods

        public void SaveHistory()
        {
            using var db = new AppDbContext();

            if (SelectedWallpaper.Id == 0)
            {
                db.Wallpapers.Add(SelectedWallpaper);
                db.SaveChanges();
            }

            var todayHistory = db.History.Any(x => x.WallpaperId == SelectedWallpaper.Id && x.AddedDate.Date != DateTime.Now.Date);

            if (!todayHistory)
            {
                var history = new History
                {
                    WallpaperId = SelectedWallpaper.Id,
                    WallpaperUId = SelectedWallpaper.UId
                };

                db.History.Add(history);
                db.SaveChanges();

                SelectedWallpaper.History.Add(history);
            }
        }

        #endregion
    }
}