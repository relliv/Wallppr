using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using Microsoft.EntityFrameworkCore;
using System.Windows.Media.Imaging;
using Wallppr.Models.Model;
using Wallppr.Helpers;
using Wallppr.Models.AppSetting;
using Wallppr.Models.Wallpaper.Entities;
using Wallppr.Models.Wallpaper.Enums;

namespace Wallppr.Data
{
    public sealed class AppDbContext : DbContext
    {
        public AppDbContext()
        {
            if (!Database.EnsureCreated()) return;

            AppSettings.Add(new AppSetting
            {
                SettingName = "CurrentLang",
                Value = "en-Us",
                IsEditable = false,
                DefaultValue = "en-Us"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "IsAppConfigured",
                Value = "0",
                IsEditable = false,
                DefaultValue = "0"
            });

            AppSettings.Add(new AppSetting 
            { 
                SettingName = "DesktopRandomWallpaperResolution",
                Value = "1920x1080",
                IsEditable = true,
                DefaultValue = "1920x1080"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "DesktopLastestWallpaperResolution",
                Value = "1920x1080",
                IsEditable = true,
                DefaultValue = "1920x1080"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "MobileRandomWallpaperResolution",
                Value = "1080x1920",
                IsEditable = true,
                DefaultValue = "1080x1920"
            });

            AppSettings.Add(new AppSetting
            {
                SettingName = "MobileLatestWallpaperResolution",
                Value = "1080x1920",
                IsEditable = true,
                DefaultValue = "1080x1920"
            });

            SaveChangesAsync();
        }


        #region AppSettings

        public DbSet<AppSetting> AppSettings { get; set; }

        #endregion

        #region Wallpaper

        public DbSet<Wallpaper> Wallpapers { get; set; }
        public DbSet<Models.Wallpaper.Entities.Color> Colors { get; set; }
        public DbSet<History> History { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
            optionsBuilder.UseSqlite($"Data Source={Settings.AppDatabaseFile};");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region AppSetting

            modelBuilder.Entity<AppSetting>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.IsEditable)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
            });

            #endregion

            #region Wallpapers

            // Wallpaper
            modelBuilder.Entity<Wallpaper>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
                entity.Property(x => x.WallpaperType)
                    .HasConversion(c => c.ToString(),
                        c => (WallpaperType)Enum.Parse(typeof(WallpaperType), c))
                    .IsUnicode(false);
                entity.Property(x => x.IsFavorite)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
                entity.Property(x => x.IsDownloaded)
                    .HasConversion(c => Convert.ToInt32(c),
                        c => Convert.ToBoolean(c));
                entity.Property(x => x.Thumbnail)
                    .HasConversion(c => c.BitmapImageToPath(),
                        c => c.PathToBitmapImage());
            });

            // Color
            modelBuilder.Entity<Models.Wallpaper.Entities.Color>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            // History
            modelBuilder.Entity<History>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.Property(x => x.AddedDate)
                    .HasConversion(c => c.ToString("yyyy-MM-dd HH:mm:ss", Settings.CultureInfo),
                        c => DateTime.Parse(c));
            });

            #endregion


            base.OnModelCreating(modelBuilder);
        }
    }
}