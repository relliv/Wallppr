using GalaSoft.MvvmLight;
using Wallppr.Data;
using Wallppr.Helpers;
using Wallppr.Models.AppSetting;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using static Wallppr.DI.DI;
using System.Collections.Generic;
using Wallppr.UI.i18N;
using Wallppr.Dialogs;

namespace Wallppr.ViewModel.App
{
    public class AppSettingsViewModel : ViewModelBase
    {
        public AppSettingsViewModel()
        {
            LanguageChangedCommand = new RelayParameterizedCommand(LanguageChanged);
            SaveSettingCommand = new RelayParameterizedCommand(SaveSetting);

            using var db = new AppDbContext();
            var Settings = db.AppSettings.OrderByDescending(x => x.IsEditable)
            .ToObservableCollection();

            UserSettings = Settings.Where(x => x.IsEditable).ToObservableCollection();
            AppSettings = Settings.Where(x => x.IsEditable == false).ToObservableCollection();

            AvailableLanguages = i18N.GetAvailableLangauges();
            SelectedLanguage = i18N.GetCurrentCultureName();
        }

        #region Commands

        public ICommand LanguageChangedCommand { get; set; }
        public ICommand SaveSettingCommand { get; set; }

        #endregion

        #region Properties

        public ObservableCollection<AppSetting> UserSettings { get; set; }
        public ObservableCollection<AppSetting> AppSettings { get; set; }

        public List<string> AvailableLanguages { get; set; }
        public string SelectedLanguage { get; set; }

        #endregion

        #region Methods


        #endregion

        #region Command Methods

        /// <summary>
        /// Language changed
        /// </summary>
        /// <param name="sender"></param>
        public void LanguageChanged(object sender)
        {
            if (string.IsNullOrEmpty((string)sender)) return;

            i18N.SwitchCurrentLanguage((string)sender);
            ViewModelApplication.LanguageResourceDictionary = i18N.GetCurrentLanguage();

            var dialog = new MessageDialog();
            dialog.ShowDialogWindow(new MessageDialogViewModel(
                dialog,
                ViewModelApplication.LanguageResourceDictionary["Info"].ToString(),
                ViewModelApplication.LanguageResourceDictionary["LanguageChangeMessage"].ToString()));
        }

        /// <summary>
        /// Save app setting
        /// </summary>
        /// <param name="sender"></param>
        public void SaveSetting(object sender)
        {
            var appSetting = (sender as TextBox).DataContext as AppSetting;

            using var db = new AppDbContext();
            db.AppSettings.Update(appSetting);
            db.SaveChanges();

            ViewModelApplication.AppSettings.LoadSettings();
        }

        #endregion
    }
}
