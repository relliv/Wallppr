using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Wallppr.Data;

namespace Wallppr.UI.i18N
{
    public static class i18N
    {
        /// <summary>
        /// Culture info string regex
        /// Supports both of en and en-US
        /// </summary>
        public static readonly string CultureInfoRegex = @"([a-z]{2})(-[A-Z]{2})?";

        /// <summary>  
        /// Set language based on previously save language setting,  
        /// otherwise set to OS lanaguage  
        /// </summary>  
        /// <param name="element"></param>  
        public static void LoadCurrentLanguage()
        {
            var langFile = GetLangFile(GetCurrentCultureName());

            SetLanguageResourceDictionary(langFile);
        }

        /// <summary>  
        /// Dynamically load a Localization ResourceDictionary from a file  
        /// </summary>  
        public static void SwitchCurrentLanguage(string inFiveCharLang)
        {
            using var db = new AppDbContext();

            var defaultLang = "en-US";
            var currentLangSetting = db.AppSettings.Where(x => x.SettingName == "CurrentLang").FirstOrDefault();

            currentLangSetting.Value = inFiveCharLang != null && Regex.Match(inFiveCharLang, CultureInfoRegex).Success
            ? inFiveCharLang
            : defaultLang;

            db.SaveChanges();

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(inFiveCharLang);

            LoadCurrentLanguage();
        }

        /// <summary>
        /// Return current language as resource dictionary
        /// </summary>
        /// <returns></returns>
        public static ResourceDictionary GetCurrentLanguage()
        {
            var langFile = GetLangFile(GetCurrentCultureName());

            if (File.Exists(langFile))
            {
                return new ResourceDictionary
                {
                    Source = new Uri(langFile)
                };
            }

            return null;
        }

        /// <summary>
        /// Returns available langauges
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAvailableLangauges()
        {
            var langFolder = Path.Combine(Settings.CurrentDirectory, "UI/i18N");

            return Directory.GetFiles(langFolder)
            .Where(x => x.Contains(".xaml"))
            .Select(x => Path.GetFileNameWithoutExtension(x).Replace("Lang-", null))
            .ToList();
        }

        /// <summary>  
        /// Get current culture info name base on previously saved setting if any,  
        /// otherwise get from OS language  
        /// </summary>  
        /// <param name="element"></param>  
        /// <returns></returns>  
        public static string GetCurrentCultureName()
        {
            using var db = new AppDbContext();

            var currentLang = "en-US";
            var currentLangSetting = db.AppSettings.Where(x => x.SettingName == "CurrentLang").FirstOrDefault();

            if (currentLangSetting != null)
            {
                if (currentLangSetting.Value != null && Regex.Match(currentLangSetting.Value, CultureInfoRegex).Success)
                {
                    currentLang = currentLangSetting.Value;
                }
                else
                {
                    currentLang = currentLangSetting.DefaultValue;
                }
            }

            return currentLang;
        }

        #region Helper MEthods

        /// <summary>
        /// Get current language file
        /// </summary>
        /// <param name="lang">target language-country code</param>
        /// <returns></returns>
        private static string GetLangFile(string lang)
        {
            return Path.Combine(Settings.CurrentDirectory, "UI/i18N", $"Lang-{lang}.xaml");
        }

        /// <summary>  
        /// Sets or replaces the ResourceDictionary by dynamically loading  
        /// a Localization ResourceDictionary from the file path passed in.  
        /// </summary>  
        /// <param name="langFile"></param>  
        private static void SetLanguageResourceDictionary(string langFile)
        {
            var element = Application.Current;

            if (File.Exists(langFile))
            {
                var languageDictionary = new ResourceDictionary
                {
                    Source = new Uri(langFile)
                };

                int langDictId = -1;
                for (int i = 0; i < element.Resources.MergedDictionaries.Count; i++)
                {
                    var mergeDictionaries = element.Resources.MergedDictionaries[i];
                    if (mergeDictionaries.Contains("ResourceDictionaryName"))
                    {
                        if (mergeDictionaries["ResourceDictionaryName"].ToString().StartsWith("Lang-"))
                        {
                            langDictId = i;
                            break;
                        }
                    }
                }

                if (langDictId == -1)
                {
                    element.Resources.MergedDictionaries.Add(languageDictionary);
                }
                else
                {
                    element.Resources.MergedDictionaries[langDictId] = languageDictionary;
                }
            }
            else
            {
                MessageBox.Show($"Language file is '{langFile}' not found.");
            }
        }

        #endregion
    }
}